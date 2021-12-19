using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace RecruitmentTask.Models
{
    public class Pyramid
    {
        private User Root { get; set; }
        private XmlDocument PyramidStructure { get; set; }
        private XmlDocument TransfersStructure { get; set; }

        public Pyramid(XmlDocument pyramid, XmlDocument transfers)
        {
            PyramidStructure = pyramid;
            TransfersStructure = transfers;
            SetRootUser();
            ApplyTransfers();
        }
        
        public void PrintUsersDataInOrder()
        {
            var allUsers = new List<User>();
            var queue = new Queue<User>();
            allUsers.Add(Root);
            queue.Enqueue(Root);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                
                foreach (var child in current.Childs)
                {
                    queue.Enqueue(child);
                    allUsers.Add(child);
                }
            }
            
            allUsers.OrderBy(x => x.Id).ToList().ForEach(PrintDataForUser);
        }

        private void PrintDataForUser(User user)
        {
            Console.WriteLine($"{user.Id} {user.Level} {user.GetChildrenWithoutChildren()} {user.Commission}");
        }

        private void SetRootUser()
        {
            var rootUser = PyramidStructure.SelectSingleNode("piramida/uczestnik");

            Root = new()
            {
                Id = 1,
                Level = 0,
                Commission = 0,
                Parent = null,
                Childs = new List<User>()
            };

            InitializePyramidWithRoot(rootUser);
        }

        private void InitializePyramidWithRoot(XmlNode rootUser)
        {
            foreach (XmlNode item in rootUser.ChildNodes)
            {
                ParseNodeToUser(Root, item);
            }           
        }

        private void ApplyTransfers()
        {
            var rootElement = TransfersStructure.SelectSingleNode("przelewy");
            
            Transfer transfer;

            foreach (XmlNode item in rootElement.ChildNodes)
            {
                transfer = new Transfer()
                {
                    From = Convert.ToInt32(item.Attributes["od"].Value),
                    Amount = Convert.ToDecimal(item.Attributes["kwota"].Value)
                };

                CalculateFeesForTransfer(transfer);
            }
        }

        private void CalculateFeesForTransfer(Transfer transfer)
        {
            var user = Find(Root, transfer.From);
            if (user != null)
            {
                var parents = GetAllParentsFromRootToUser(user);
                Pay(parents, transfer.Amount);
            }
        }
        
        private void Pay(IList<User> users, decimal amount) 
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (i == users.Count - 1)
                    users[i].Commission += amount;
                else
                {
                    var amountToPay = Math.Floor(Decimal.Divide(amount, 2));
                    amount -= amountToPay;
                    users[i].Commission += amountToPay;
                }
            }
        }

        private void ParseNodeToUser(User parent, XmlNode node)
        {
            var user = new User()
            {
                Id = Convert.ToInt32(node.Attributes["id"].Value),
                Level = parent.Level+1,
                Commission = 0,
                Parent = parent,
                Childs = new List<User>(),
            };

            parent.Childs.Add(user);

            foreach (XmlNode item in node.ChildNodes)
            {
                ParseNodeToUser(user, item);
            }
        }
        
        private IList<User> GetAllParentsFromRootToUser(User user)
        {
            var userParents = new List<User>();
            var parent = user.Parent;
            
            while (parent is not null)
            {
                userParents.Add(parent);
                parent = parent.Parent;
            }

            userParents.Reverse();
            return userParents;
        }
        private User Find(User user, int indexToFind)
        {
            if (user.Id == indexToFind)
                return user;

            foreach (var child in user.Childs) 
            { 
                var result = Find(child, indexToFind);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}