﻿using OpenBound_Network_Object_Library.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenBound_Network_Object_Library.FileManagement.Versioning
{
    public class PatchEntry
    {
        public Guid ID;
        public DateTime ReleaseDate;
        public string PatchPath;
        public string PatchVersionName;
    }

    public class PatchHistory
    {
        public List<PatchEntry> PatchEntryList;

        public PatchHistory()
        {
            PatchEntryList = new List<PatchEntry>();
        }

        public void AddPatchEntry(ApplicationManifest applicationManifest)
        {
            PatchEntryList.Add(new PatchEntry()
            {
                ID = applicationManifest.ID,
                ReleaseDate = applicationManifest.CreationDate,
                PatchPath = applicationManifest.BuildPatchPath,
                PatchVersionName = applicationManifest.PatchVersionName
            });
        }

        public void MergePatchEntry(ApplicationManifest appManifest1, ApplicationManifest appManifest2, ApplicationManifest newAppManifest)
        {
            PatchEntry previousPE1 = null, previousPE2 = null;

            for(int i = 0; i < PatchEntryList.Count - 1; i++)
            {
                if (PatchEntryList[i].ID == appManifest1.ID && PatchEntryList[i + 1].ID == appManifest2.ID)
                {
                    previousPE1 = PatchEntryList[i];
                    previousPE2 = PatchEntryList[i + 1];
                    break;
                }
            }

            PatchEntryList.Remove(previousPE1);

            previousPE2.ID = appManifest2.ID;
            previousPE2.PatchPath = appManifest2.BuildPatchPath;
            previousPE2.ReleaseDate = appManifest2.CreationDate;
            previousPE2.PatchVersionName = newAppManifest.PatchVersionName;
        }

        public static PatchHistory CreatePatchHistoryInstance(string previousPatchHistoryPath = null)
        {
            string fileContent = File.ReadAllText(previousPatchHistoryPath);
            return ObjectWrapper.Deserialize<PatchHistory>(fileContent);
        }
    }
}