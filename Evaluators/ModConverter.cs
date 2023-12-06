using System;
using System.Linq;
using System.Collections.Generic;

namespace osu_tops.Evaluators
{

	public class ModConverter
	{

		// Rankable mods that can be submitted in scores

		public enum ModValues
		{
			NM = 0,
			NF = 1,
			EZ = 2,
			HD = 8,
			HR = 16,
			SD = 32,
			DT = 64,
			HT = 256,
			NC = 512,
			FL = 1024,
			SO = 4096,
			PF = 16384,
		}

		// Return string form of "enabled_mods" provided by API
		public static string modToString(int enabledMods)
		{

			if (enabledMods == (int)ModValues.NM)
			{
				return "NM";
			}

			ModValues[] modValues = (ModValues[])Enum.GetValues(typeof(ModValues));
			// Evaluate starting from largest values PF down to NF
			Array.Reverse(modValues);
			string modString = "";

			foreach (ModValues currModValue in modValues)
			{
				if ((enabledMods & (int)currModValue) != 0)
				{
					// NC and DT are exclusive
					if (currModValue == ModValues.NC)
					{
						enabledMods -= (int)ModValues.DT;
					}

					modString += currModValue.ToString();
				}
			}

			return rewriteOrder(modString);
		}

		// Remove mods that recommendation algorithm won't consider
		// Includes: PF, SO, SD, NF
		public static string simplifyMods(string modString) {
			string newModString = modString
				.Replace("NF", "")
				.Replace("SO", "")
				.Replace("SD", "")
				.Replace("PF", "")
				.Replace("NC", "DT");

            return newModString;
		}

		// Standardise written order of mods
		private static string rewriteOrder(string modString) {
            Dictionary<string, int> modPriority = new Dictionary<string, int> {
				{ "SO", 0 },
				{ "EZ", 1 },
				{ "NF", 2 },
				{ "HD", 3 },
				{ "DT", 4 },
				{ "NC", 4 },
				{ "HT", 4 },
				{ "HR", 5 },
				{ "SD", 6 },
				{ "PF", 6 },
                { "FL", 7 },
            };

			// Every two consecutive letters in "modString" is a mod, store all mods in a list
            List<string> modsList = Enumerable.Range(0, modString.Length / 2).Select(i => modString.Substring(i * 2, 2)).ToList();
			// Sort mods based off priority
            modsList.Sort((a, b) => modPriority[a].CompareTo(modPriority[b]));

			//Concatenate list back to string format
			return string.Join("", modsList);
        }
    }
}
