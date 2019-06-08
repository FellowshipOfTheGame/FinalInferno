using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Data;

namespace FinalInferno{
    public class DynamicTable : DataTable
    {
        public static DynamicTable Create(TextAsset textFile){
            if(textFile == null){
                Debug.Log("Must pass a valid textFile as parameter");
                return null;
            }else
                return new DynamicTable(textFile);
        }
        protected DynamicTable(TextAsset textFile) : base(){
            string[] lines = Regex.Split(textFile.text, "\n|\r\n|\r");
            int nCols = lines[0].Split(',').Length;
            // Create table columns
            for(int i = 0; i < nCols; i++){
                string colHeader = lines[0].Split(',')[i];
                string colType = lines[1].Split(',')[i];
                // Safeguard for types from different assembly file
                switch(colType){
                    case "string":
                        colType = typeof(string).AssemblyQualifiedName;
                        break;
                    case "int":
                        colType = typeof(int).AssemblyQualifiedName;
                        break;
                    case "long":
                        colType = typeof(long).AssemblyQualifiedName;
                        break;
                    case "float":
                        colType = typeof(float).AssemblyQualifiedName;
                        break;
                    case "bool":
                        colType = typeof(bool).AssemblyQualifiedName;
                        break;
                    case "Color":
                        colType = typeof(Color).AssemblyQualifiedName;
                        break;
                    case "Enemy":
                        colType = typeof(Enemy).AssemblyQualifiedName;
                        break;
                    case "Party":
                        colType = typeof(Party).AssemblyQualifiedName;
                        break;
                    case "Skill":
                        colType = typeof(Skill).AssemblyQualifiedName;
                        break;
                    default:
                        colType = typeof(string).AssemblyQualifiedName;
                        break;
                }
                Columns.Add(new DataColumn(colHeader, System.Type.GetType(colType)));
            }
            // Add lines to table
            for(int i = 2; i < lines.Length; i++){
                DataRow newRow = NewRow();
                string[] elements = lines[i].Split(',');
                for(int j = 0; j < elements.Length; j++){
                    AddElement(ref newRow, Columns[j].ColumnName, elements[j], Columns[j].DataType);
                }
                Rows.Add(newRow);
            }
        }
        protected void AddElement(ref DataRow row, string colName, string description, System.Type type){
            if(type == typeof(int)){
                //Debug.Log("parsing " + description + " as int for column " + colName + " and row " + row.ToString());
                row[colName] = int.Parse(description);
            }else if(type == typeof(long)){
                row[colName] = long.Parse(description);
            }else if(type == typeof(string)){
                row[colName] = description;
            }else if(type == typeof(float)){
                row[colName] = float.Parse(description);
            }else if(type == typeof(string)){
                row[colName] = description;
            }else if(type == typeof(bool)){
                row[colName] = bool.Parse(description);
            }else if(type == typeof(Color)){
                Color newColor = new Color();
                ColorUtility.TryParseHtmlString(description, out newColor);
                row[colName] = newColor;
            }else{
                row[colName] = AssetManager.LoadAsset(description, type);
            }
        }
    }
}
