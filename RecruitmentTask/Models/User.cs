using System.Collections;
using System.Collections.Generic;

namespace RecruitmentTask
{
    public class User
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public decimal Commission { get; set; } = 0;

        public User Parent { get; set; }
        public IList<User> Childs { get; set; }

        public int ChildrenWithoutChildren()
        {
            var queue = new Queue<User>();
            queue.Enqueue(this);

            var count = 0;
            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                count += CountChildrenWithoutChildren(current.Childs);
                foreach (var child in current.Childs)
                {
                    queue.Enqueue(child);
                }
            }

            return count;
        }

        private int CountChildrenWithoutChildren(IList<User> children)
        {
            var count = 0;
            foreach (var child in children)
            {
                if (child.Childs.Count == 0)
                    count++;
            }

            return count;
        }
    }
}