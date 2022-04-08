namespace FinalInferno.CustomExtensions {
    public static class CustomStringExtensions {

        private struct VersionValue {
            public int chapter;
            public int major;
            public int minor;
            public VersionValue(int chapter, int major, int minor) {
                this.chapter = chapter;
                this.major = major;
                this.minor = minor;
            }
            public bool IsOlderThan(VersionValue other) {
                return chapter < other.chapter
                    || chapter == other.chapter && (major < other.major || major == other.major && minor < other.minor);
            }
            public bool IsNewerThan(VersionValue other) {
                return chapter > other.chapter
                    || chapter == other.chapter && (major > other.major || major == other.major && minor > other.minor);
            }
        }

        private static VersionValue ParseVersionValue(string versionString) {
            string exceptionPrefix = $"Can't parse string {versionString} as version value: ";
            if (string.IsNullOrEmpty(versionString)) {
                return new VersionValue(0, 0, 0);
            }

            string[] numbers = versionString.Split('.');
            if (numbers.Length < 3) {
                throw new System.ArgumentException($"{exceptionPrefix} string is missing components");
            }

            int chapter;
            int major;
            int minor;
            try {
                chapter = int.Parse(numbers[0]);
                major = int.Parse(numbers[1]);
                minor = int.Parse(numbers[2]);
            } catch (System.Exception e) {
                throw new System.ArgumentException($"{exceptionPrefix} error parsing part of string to int", e);
            }

            return new VersionValue(chapter, major, minor);
        }


        private static bool IsFirstOlderThanSecond(string first, string second) {
            VersionValue firstVersionValue = ParseVersionValue(first);
            VersionValue secondVersionValue = ParseVersionValue(second);

            return firstVersionValue.IsOlderThan(secondVersionValue);
        }

        private static bool IsFirstNewerThanSecond(string first, string second) {
            VersionValue firstVersionValue = ParseVersionValue(first);
            VersionValue secondVersionValue = ParseVersionValue(second);

            return firstVersionValue.IsNewerThan(secondVersionValue);
        }

        public static bool IsOlderVersionThan(this string thisString, string comparedString) {
            return IsFirstOlderThanSecond(thisString, comparedString);
        }

        public static bool IsNewerVersionThan(this string thisString, string comparedString) {
            return IsFirstNewerThanSecond(thisString, comparedString);
        }
    }
}