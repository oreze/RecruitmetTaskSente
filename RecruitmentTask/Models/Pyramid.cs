using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace RecruitmentTask
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
            InitializePyramid();
            ApplyTransfers();
            LevelOrderTraversal();
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

                var user = Find(Root, transfer.From);
                if (user != null)
                {
                    var parents = GetAllParentsFromRootToUser(user);
                    Pay(parents, transfer.Amount);
                }
            }
        }

        /// <summary>
        /// Pays users their commision 
        /// </summary>
        /// <param name="user">Users to pay off</param>
        /// <param name="amount">The money we have to pay them off</param>
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

        private IList<User> GetAllParentsFromRootToUser(User user)
        {
            var parents = new List<User>();

            var parent = user.Parent;
            while (parent is not null)
            {
                parents.Add(parent);
                parent = parent.Parent;
            }

            parents.Reverse();
            return parents;
        }
        
        public void LevelOrderTraversal()
        {
            var queue = new Queue<User>();
            queue.Enqueue(Root);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                Console.WriteLine($"{current.Id} {current.Level} {current.ChildrenWithoutChildren()} {current.Commission}");
                foreach (var child in current.Childs)
                {
                    queue.Enqueue(child);
                }
            }
        }

        private void InitializePyramid()
        {
            SetRootUser();
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

            foreach (XmlNode item in rootUser.ChildNodes)
            {
                ParseNodeToUser(Root, item);
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