//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
//using System.Data;
using System.Globalization;
using System.Collections.ObjectModel;

namespace FinalInferno{
    [System.Serializable]
    public class DynamicTable/* : DataTable*/
    {
        // Declaração de subclasses e delegates ------------------
        [System.Serializable]
        private class ColDescription : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<string, int>{ }

        [System.Serializable]
        public class TableRow {
            [SerializeField] private ColNumberDelegate GetColNumber;
            public int Count { get{ return (elements != null)? elements.Length : 0; }}
            [SerializeField] private string[] elements;

            public TableRow(string line, ColNumberDelegate colDelegate){
                elements = line.Split(splitCharacter);
                GetColNumber = colDelegate;
            }

            public T Field<T>(string colName){
                int colNumber = GetColNumber(colName, typeof(T).AssemblyQualifiedName);
                if(colNumber < 0) return default(T);

                string description = elements[colNumber];

                if(typeof(T) == typeof(int)){
                    //Debug.Log("parsing " + description + " as int for column " + colName + " and row " + row.ToString());
                    return (T)(object)int.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                }else if(typeof(T) == typeof(long)){
                    return (T)(object)long.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                }else if(typeof(T) == typeof(string)){
                    return (T)(object)description;
                }else if(typeof(T) == typeof(float)){
                    return (T)(object)float.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                }else if(typeof(T) == typeof(bool)){
                    return (T)(object)bool.Parse(description);
                }else if(typeof(T) == typeof(Color)){
                    Color newColor = new Color();
                    ColorUtility.TryParseHtmlString(description, out newColor);
                    return (T)(object)newColor;
                }else{
                    return (T)(object)AssetManager.LoadAsset(description, typeof(T));
                }
            }
        }

        public delegate int ColNumberDelegate(string colName, string typeQualName);

        // Variaveis/Proriedades -------------------------
        private const char splitCharacter = ',';
        [SerializeField] private string[] colTypes;
        [SerializeField] private ColDescription Col;
        [SerializeField] private TableRow[] rows;
        public ReadOnlyCollection<TableRow> Rows { get{ return new List<TableRow>(rows).AsReadOnly(); } }

        // Metodos ---------------------------------------
        public static DynamicTable Create(TextAsset textFile){
            if(textFile == null){
                Debug.Log("Must pass a valid textFile as parameter");
                return null;
            }else
                return new DynamicTable(textFile);
        }

        protected static private string GetQualifiedName(string typeName){
            switch(typeName){
                case "string":
                    return typeof(string).AssemblyQualifiedName;
                case "int":
                    return typeof(int).AssemblyQualifiedName;
                case "long":
                    return typeof(long).AssemblyQualifiedName;
                case "float":
                    return typeof(float).AssemblyQualifiedName;
                case "bool":
                    return typeof(bool).AssemblyQualifiedName;
                case "Color":
                    return typeof(Color).AssemblyQualifiedName;
                case "Enemy":
                    return typeof(Enemy).AssemblyQualifiedName;
                case "Party":
                    return typeof(Party).AssemblyQualifiedName;
                case "Skill":
                    return typeof(Skill).AssemblyQualifiedName;
                default:
                    return typeof(string).AssemblyQualifiedName;
            }
        }

        public int GetColNumber(string colName, string assembQualName){
            return Col[colName];
        }

        protected DynamicTable(TextAsset textFile){
            string[] lines = Regex.Split(textFile.text, "\n|\r\n|\r");
            string[] colNames = lines[0].Split(splitCharacter);
            string[] colTypeNames = lines[1].Split(splitCharacter);

            Col = new ColDescription();
            colTypes = new string[colTypeNames.Length];
            rows = new TableRow[lines.Length - 2];

            int nCols = colNames.Length;
            for(int i = 0; i < colNames.Length; i++){
                Col.Add(colNames[i], i);
                colTypes[i] = GetQualifiedName(colTypeNames[i]);
            }

            for(int i = 2; i < lines.Length; i++){
                rows[i] = new TableRow(lines[i], GetColNumber);
            }
        }

        // protected DynamicTable(TextAsset textFile) : base(){
        //     string[] lines = Regex.Split(textFile.text, "\n|\r\n|\r");
        //     int nCols = lines[0].Split(splitCharacter).Length;
        //     // Create table columns
        //     for(int i = 0; i < nCols; i++){
        //         string colHeader = lines[0].Split(splitCharacter)[i];
        //         string colType = lines[1].Split(splitCharacter)[i];
        //         // Safeguard for types from different assembly file
        //         switch(colType){
        //             case "string":
        //                 colType = typeof(string).AssemblyQualifiedName;
        //                 break;
        //             case "int":
        //                 colType = typeof(int).AssemblyQualifiedName;
        //                 break;
        //             case "long":
        //                 colType = typeof(long).AssemblyQualifiedName;
        //                 break;
        //             case "float":
        //                 colType = typeof(float).AssemblyQualifiedName;
        //                 break;
        //             case "bool":
        //                 colType = typeof(bool).AssemblyQualifiedName;
        //                 break;
        //             case "Color":
        //                 colType = typeof(Color).AssemblyQualifiedName;
        //                 break;
        //             case "Enemy":
        //                 colType = typeof(Enemy).AssemblyQualifiedName;
        //                 break;
        //             case "Party":
        //                 colType = typeof(Party).AssemblyQualifiedName;
        //                 break;
        //             case "Skill":
        //                 colType = typeof(Skill).AssemblyQualifiedName;
        //                 break;
        //             default:
        //                 colType = typeof(string).AssemblyQualifiedName;
        //                 break;
        //         }
        //         Columns.Add(new DataColumn(colHeader, System.Type.GetType(colType)));
        //     }
        //     // Add lines to table
        //     for(int i = 2; i < lines.Length; i++){
        //         DataRow newRow = NewRow();
        //         string[] elements = lines[i].Split(splitCharacter);
        //         for(int j = 0; j < elements.Length; j++){
        //             AddElement(ref newRow, Columns[j].ColumnName, elements[j], Columns[j].DataType);
        //         }
        //         base.Rows.Add(newRow);
        //     }
        // }


        // protected void AddElement(ref DataRow row, string colName, string description, System.Type type){
        //     if(type == typeof(int)){
        //         //Debug.Log("parsing " + description + " as int for column " + colName + " and row " + row.ToString());
        //         row[colName] = int.Parse(description);
        //     }else if(type == typeof(long)){
        //         row[colName] = long.Parse(description);
        //     }else if(type == typeof(string)){
        //         row[colName] = description;
        //     }else if(type == typeof(float)){
        //         row[colName] = float.Parse(description,  NumberStyles.Float | NumberStyles.AllowDecimalPoint);
        //     }else if(type == typeof(string)){
        //         row[colName] = description;
        //     }else if(type == typeof(bool)){
        //         row[colName] = bool.Parse(description);
        //     }else if(type == typeof(Color)){
        //         Color newColor = new Color();
        //         ColorUtility.TryParseHtmlString(description, out newColor);
        //         row[colName] = newColor;
        //     }else{
        //         row[colName] = AssetManager.LoadAsset(description, type);
        //     }
        // }
    }
}
