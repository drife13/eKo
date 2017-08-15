using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eko.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eko.Data
{
    public static class DbExtensions
    {
        public static void EnsureSeedData(this ApplicationDbContext db)
        {
            if (!db.Items.Any())
            {
                string line;
                string[] splitLine;
                Category C1 = new Category();
                Category C2 = new Category();
                Category C3 = new Category();

                using (StreamReader file = File.OpenText("Data/ReverbCategories.txt"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        splitLine = line.Split(new string[] { " > " }, StringSplitOptions.None);

                        if (splitLine.Count() == 1)
                        {
                            C1 = new Category(splitLine[0]);
                            db.Categories.Add(C1);
                        }
                        if (splitLine.Count() == 2)
                        {
                            C2 = new Category(splitLine[1], C1);
                            db.Categories.Add(C2);
                        }
                        if (splitLine.Count() == 3)
                        {
                            if (C2.Name != splitLine[1])
                            {
                                C2 = new Category(splitLine[1], C1);
                                db.Categories.Add(C2);
                            }
                            C3 = new Category(splitLine[2], C2);
                            db.Categories.Add(C3);
                        }
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
