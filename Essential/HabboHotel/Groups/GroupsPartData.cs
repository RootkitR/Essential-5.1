
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;

namespace Essential
{
    internal sealed class GroupsPartsData
    {
        internal static List<GroupsPartsData> BaseBadges;
        internal static List<GroupsPartsData> ColorBadges1;
        internal static List<GroupsPartsData> ColorBadges2;
        internal static List<GroupsPartsData> ColorBadges3;
        internal string ExtraData1;
        internal string ExtraData2;
        internal int Id;
        internal static List<GroupsPartsData> SymbolBadges;

        internal static void InitGroups()
        {
            BaseBadges = new List<GroupsPartsData>();
            SymbolBadges = new List<GroupsPartsData>();
            ColorBadges1 = new List<GroupsPartsData>();
            ColorBadges2 = new List<GroupsPartsData>();
            ColorBadges3 = new List<GroupsPartsData>();
            using (DatabaseClient adapter = Essential.GetDatabase().GetClient())
            {
                DataTable table = adapter.ReadDataTable("SELECT * FROM groups_elements");
                foreach (DataRow row in table.Rows)
                {
                    GroupsPartsData data;
                    if (row["Type"].ToString() == "Base")
                    {
                        data = new GroupsPartsData
                        {
                            Id = (int)row["Id"],
                            ExtraData1 = (string)row["ExtraData1"],
                            ExtraData2 = (string)row["ExtraData2"]
                        };
                        BaseBadges.Add(data);
                    }
                    else if (row["ExtraData1"].ToString().StartsWith("symbol_"))
                    {
                        data = new GroupsPartsData
                        {
                            Id = (int)row["Id"],
                            ExtraData1 = (string)row["ExtraData1"],
                            ExtraData2 = (string)row["ExtraData2"]
                        };
                        SymbolBadges.Add(data);
                    }
                    else if (row["Type"].ToString() == "Color1")
                    {
                        data = new GroupsPartsData
                        {
                            Id = (int)row["Id"],
                            ExtraData1 = (string)row["ExtraData1"]
                        };
                        ColorBadges1.Add(data);
                    }
                    else if (row["Type"].ToString() == "Color2")
                    {
                        data = new GroupsPartsData
                        {
                            Id = (int)row["Id"],
                            ExtraData1 = (string)row["ExtraData1"]
                        };
                        ColorBadges2.Add(data);
                    }
                    else if (row["Type"].ToString() == "Color3")
                    {
                        data = new GroupsPartsData
                        {
                            Id = (int)row["Id"],
                            ExtraData1 = (string)row["ExtraData1"]
                        };
                        ColorBadges3.Add(data);
                    }
                }
            }
        }
    }
}

