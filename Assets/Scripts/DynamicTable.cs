using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public class DynamicTable : ISerializationCallbackReceiver {
        // Declaração de subclasses e delegates ------------------
        #region subclass
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
                    return colTypes[accessDict[colName]] == assembQualName ? accessDict[colName] : -1;
                } catch (KeyNotFoundException e) {
                    Debug.LogError(e.Message);
                    return -1;
                }
            }

            public bool HasField<T>(string colName) {
                return (GetColNumber(colName, typeof(T).AssemblyQualifiedName) != -1);
            }

            private T TryParseInt<T>(string fieldContent) {
                try {
                    return (T)(object)int.Parse(fieldContent, CultureInfo.InvariantCulture.NumberFormat);
                } catch {
                    Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    return default;
                }
            }

            private T TryParseLong<T>(string fieldContent) {
                try {
                    return (T)(object)long.Parse(fieldContent, CultureInfo.InvariantCulture.NumberFormat);
                } catch {
                    Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    return default;
                }
            }

            private T TryParseFloat<T>(string fieldContent) {
                try {
                    return (T)(object)float.Parse(fieldContent, CultureInfo.InvariantCulture.NumberFormat);
                } catch {
                    Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    return default;
                }
            }

            private T TryParseBool<T>(string fieldContent) {
                try {
                    return (T)(object)bool.Parse(fieldContent);
                } catch {
                    Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                    return default;
                }
            }

            private T TryParseColor<T>(string fieldContent) {
                Color newColor;
                ColorUtility.TryParseHtmlString(fieldContent, out newColor);
                return (T)(object)newColor;
            }

            private bool IsEnum(System.Type type) {
                return type == typeof(Element) || type == typeof(DamageType);
            }

            private T TryParseEnum<T>(string fieldContent) {
                int value = 0;
                try {
                    value = int.Parse(fieldContent, CultureInfo.InvariantCulture.NumberFormat);
                } catch {
                    Debug.LogError($"Error trying to parse object of type {typeof(T).AssemblyQualifiedName} from DynamicTable");
                }
                return typeof(T) == typeof(Element) ? (T)(object)(Element)value : (T)(object)(DamageType)value;
            }

            private T TryParseField<T>(string fieldContent) {
                if (typeof(T) == typeof(int)) {
                    return TryParseInt<T>(fieldContent);
                } else if (typeof(T) == typeof(long)) {
                    return TryParseLong<T>(fieldContent);
                } else if (typeof(T) == typeof(string)) {
                    return (T)(object)fieldContent;
                } else if (typeof(T) == typeof(float)) {
                    return TryParseFloat<T>(fieldContent);
                } else if (typeof(T) == typeof(bool)) {
                    return TryParseBool<T>(fieldContent);
                } else if (typeof(T) == typeof(Color)) {
                    return TryParseColor<T>(fieldContent);
                } else if (IsEnum(typeof(T))) {
                    return TryParseEnum<T>(fieldContent);
                } else {
                    throw new System.NotImplementedException();
                }
            }

            public T Field<T>(string colName) {
                int colNumber = GetColNumber(colName, typeof(T).AssemblyQualifiedName);
                if (colNumber < 0) {
                    return default;
                }

                string fieldContent = elements[colNumber];

                return AssetManager.IsTypeSupported(typeof(T))
                    ? (T)(object)AssetManager.LoadAsset(fieldContent, typeof(T))
                    : TryParseField<T>(fieldContent);
            }
        }
        #endregion

        // Variaveis/Propriedades -------------------------
        protected const char splitCharacter = ';';
        [SerializeField] private string[] colTypes;
        [SerializeField] private string[] colNames;
        private Dictionary<string, int> accessDict;
        [SerializeField] private TableRow[] rows;
        private ReadOnlyCollection<TableRow> readOnlyRows;
        public ReadOnlyCollection<TableRow> Rows => readOnlyRows;

        // Metodos ---------------------------------------
        public static DynamicTable Create(TextAsset textFile) {
            if (textFile == null) {
                Debug.LogWarning("Must pass a valid textFile as parameter");
                return null;
            } else {
                return new DynamicTable(textFile);
            }
        }

        protected static private string GetQualifiedName(string typeName) {
            return typeName switch {
                "string" => typeof(string).AssemblyQualifiedName,
                "int" => typeof(int).AssemblyQualifiedName,
                "long" => typeof(long).AssemblyQualifiedName,
                "float" => typeof(float).AssemblyQualifiedName,
                "bool" => typeof(bool).AssemblyQualifiedName,
                "Color" => typeof(Color).AssemblyQualifiedName,
                "Enemy" => typeof(Enemy).AssemblyQualifiedName,
                "Party" => typeof(Party).AssemblyQualifiedName,
                "Skill" => typeof(Skill).AssemblyQualifiedName,
                "Element" => typeof(Element).AssemblyQualifiedName,
                "DamageType" => typeof(DamageType).AssemblyQualifiedName,
                _ => typeof(string).AssemblyQualifiedName,
            };
        }

        protected void TryAddAccessDictEntry(string key, int value) {
            try {
                accessDict.Add(key, value);
            } catch (System.ArgumentException error) {
                Debug.LogError($"Table has more than one column named {key}");
                throw error;
            }
        }

        protected void SetupAccessDict() {
            accessDict = new Dictionary<string, int>();
            if (colNames != null) {
                for (int i = 0; i < colNames.Length; i++) {
                    TryAddAccessDictEntry(colNames[i], i);
                }
            }
            for (int i = 0; i < rows.Length; i++) {
                rows[i].accessDict = accessDict;
            }
        }

        protected DynamicTable(TextAsset textFile) {
            string[] lines = textFile.text.Split((new char[] { '\n', '\r' }), System.StringSplitOptions.RemoveEmptyEntries);
            colNames = lines[0].Split(splitCharacter);
            string[] colTypeNames = lines[1].Split(splitCharacter);
            colTypes = new string[colTypeNames.Length];
            rows = new TableRow[lines.Length - 2];

            for (int i = 0; i < colNames.Length; i++) {
                colTypes[i] = GetQualifiedName(colTypeNames[i]);
            }
            for (int i = 2; i < lines.Length; i++) {
                rows[i - 2] = new TableRow(lines[i], colTypes);
            }
            SetupAccessDict();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            SetupAccessDict();
            readOnlyRows = rows == null ? (new List<TableRow>()).AsReadOnly() : (new List<TableRow>(rows)).AsReadOnly();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            readOnlyRows = null;
        }
    }
}
