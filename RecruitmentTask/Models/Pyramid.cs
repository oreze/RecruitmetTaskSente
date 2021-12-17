using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace RecruitmentTask
{
    public class Pyramid
    {
        private User Root { get; set; }
        private XmlDocument Pyramid { get; set; }
        private IList<Transfer> Transfers { get; set; }

        public Pyramid(XmlDocument pyramid, XmlDocument transfers)
        {
            Pyramid = pyramid;
            InitializeTransfers(transfers);
            InitializePyramid();
        }

        private void InitializeTransfers(XmlDocument transfersDoc)
        {
            var transfers = new List<Transfer>();

            foreach (XmlNode item in transfersDoc.ChildNodes)
            {
                transfers.Add(new Transfer()
                {
                    From = Convert.ToInt32(item.Attributes["od"]),
                    Amount = Convert.ToDecimal(item.Attributes["kwota"])
                });
            }
        }

        private void InitializePyramid()
        {
            SetRootUser();
        }

        private void SetRootUser()
        {
            var rootUser = Pyramid.SelectSingleNode("piramida/uczestnik");

            Root = new()
            {
                Id = 1,
                Childs = new List<User>(),
                Commission = 0,
                Level = 0
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
                Childs = new List<User>(),
                Commission = 0,
                Level = ++parent.Level
            };

            parent.Childs.Add(user);

            foreach (XmlNode item in node.ChildNodes)
            {
                ParseNodeToUser(user, item);
            }
        }
        
        
    }
}