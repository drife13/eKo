using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public string URLRoute {
            get { return FullName.ToLower().Replace(" > ", "/").Replace(' ', '-'); }
        }

        public string URLName
        {
            get { return Name.ToLower().Replace(' ', '-') + ID; }
        }

        public int ParentId { get; set; }
        public int GrandParentId { get; set; }
        public IList<Category> Children { get; set; }
        public int Level { get; set; }

        public string DisplayText
        {
            get { return string.Concat(new string('-', Level * 2), Name); }
        }

        public Category() { }

        public Category(string name)
        {
            Name = name;
            FullName = name;
            Level = 1;
        }

        public Category(string name, Category parent) : this(name)
        {
            Level = parent.Level + 1;
            FullName = parent.FullName + " > " + name;
            ParentId = parent.ID;
            if (Level == 3) { GrandParentId = parent.ParentId; }
        }

        public Category(string name, IList<Category> list) : this(name)
        {
            list.Add(this);
        }

        public Category(string name, Category parent, IList<Category> list) : this(name, parent)
        {
            list.Add(this);
        }
    }
}
