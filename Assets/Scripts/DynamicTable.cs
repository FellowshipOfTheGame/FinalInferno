using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public class DynamicTable : ISerializationCallbackReceiver {
        // Declaração de subclasses e delegates ------------------
        [System.Serializable]
        public class TableRow {
            public int Count => (elements != null) ? elements.Length : 0;
            [SerializeField] private string[] elements;
            [HideInInspector, SerializeField] private string[] colTypes;
            public Dictionary<string, int> accessDict;

            public TableRow(string line, string[] types) {
                elements = line.Split(splitCharacter);
                colTypes = types;
            }

            private int GetColNumber(string colName, string assembQualName) {
                if (accessDict == null) {
                    Debug.LogError("Access dictionary is not set in TableRow");
                    return -1;
                }

                try {
                    if (colTypes[accessDict[colName]] == assembQualName) {
                        return accessDict[colName];
                    } else {
                        return -1;
                    }
                } catch (KeyNotFoundException e) {
                    Debug.LogError(e.Message);
                    return -1;
                }
            }

            public bool HasField<T>(string colName) {
                return (GetColNumber(colName, typeof(T).AssemblyQualifiedName) != -1);
            }

            public T Field<T>(string colName) {
                int colNumber = GetColNumber(colName, typeof(T).AssemblyQualifiedName);
                if (colNumber < 0) {
                    return default(T);
                }

                string description = elements[colNumber];

                if (typeof(T) == typeof(int)) {
                    try {
                        return (T)(object)int.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                    } catch {
                        Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    }
                } else if (typeof(T) == typeof(long)) {
                    try {
                        return (T)(object)long.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                    } catch {
                        Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    }
                } else if (typeof(T) == typeof(string)) {
                    return (T)(object)description;
                } else if (typeof(T) == typeof(float)) {
                    try {
                        return (T)(object)float.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                    } catch {
                        Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    }
                } else if (typeof(T) == typeof(bool)) {
                    try {
                        return (T)(object)bool.Parse(description);
                    } catch {
                        Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    }
                } else if (typeof(T) == typeof(Color)) {
                    Color newColor = new Color();
                    ColorUtility.TryParseHtmlString(description, out newColor);
                    return (T)(object)newColor;
                } else if (typeof(T) == typeof(Element) || typeof(T) == typeof(DamageType)) {
                    int value = 0;
                    try {
                        value = int.Parse(description, CultureInfo.InvariantCulture.NumberFormat);
                    } catch {
                        Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    }
                    if (typeof(T) == typeof(Element)) {
                        return (T)(object)(Element)value;
                    } else {
                        return (T)(object)(DamageType)value;
                    }
                } else {
                    return (T)(object)AssetManager.LoadAsset(description, typeof(T));
                }

                return default(T);
            }
        }

        // Variaveis/Proriedades -------------------------
        private const char splitCharacter = ';';
        [SerializeField] private string[] colTypes;
        [SerializeField] private string[] colNames;
        private Dictionary<string, int> accessDict;
        [SerializeField] private TableRow[] rows;
        public ReadOnlyCollection<TableRow> Rows {
            get {
                if (rows == null) {
                    return (new List<TableRow>()).AsReadOnly();
                } else {
                    return (new List<TableRow>(rows)).AsReadOnly();
                }
            }
        }

        // Metodos ---------------------------------------
        public static DynamicTable Create(TextAsset textFile) {
            if (textFile == null) {
                Debug.LogWarning("Must pass a valid textFile as parameter");
                return null;
            } else {
                return new DynamicTable(textFile);
            }
        }

        // public void Clear(){
        //     colTypes = new string[0];
        //     if(accessDict != null){
        //         accessDict.Clear();
        //     }
        //     rows = new TableRow[0];
        // }

        protected static private string GetQualifiedName(string typeName) {
            switch (typeName) {
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
                case "Element":
                    return typeof(Element).AssemblyQualifiedName;
                case "DamageType":
                    return typeof(DamageType).AssemblyQualifiedName;
                default:
                    return typeof(string).AssemblyQualifiedName;
            }
        }

        protected DynamicTable(TextAsset textFile) {
            string[] lines = textFile.text.Split((new char[] { '\n', '\r' }), System.StringSplitOptions.RemoveEmptyEntries);
            colNames = lines[0].Split(splitCharacter);
            string[] colTypeNames = lines[1].Split(splitCharacter);

            accessDict = new Dictionary<string, int>();
            colTypes = new string[colTypeNames.Length];
            rows = new TableRow[lines.Length - 2];

            int nCols = colNames.Length;
            for (int i = 0; i < colNames.Length; i++) {
                try {
                    accessDict.Add(colNames[i], i);
                } catch (System.ArgumentException error) {
                    Debug.LogError($"Table has more than one column named {colNames[i]}");
                    throw error;
                }
                colTypes[i] = GetQualifiedName(colTypeNames[i]);
            }

            for (int i = 2; i < lines.Length; i++) {
                rows[i - 2] = new TableRow(lines[i], colTypes);
                rows[i - 2].accessDict = accessDict;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            accessDict = new Dictionary<string, int>();
            if (colNames != null) {
                for (int i = 0; i < colNames.Length; i++) {
                    try {
                        accessDict.Add(colNames[i], i);
                    } catch (System.ArgumentException error) {
                        Debug.LogError($"Table has more than one column named {colNames[i]}");
                        throw error;
                    }
                }
            }

            for (int i = 0; i < rows.Length; i++) {
                rows[i].accessDict = accessDict;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
        }
    }
}
