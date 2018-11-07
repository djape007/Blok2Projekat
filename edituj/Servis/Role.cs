﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Servis {

    public enum Roles {
        Admin = 1,
        Reader,
        Writer
    };    
    public enum Permissions {
        CreateDB = 1,
        DeleteDB,
        ReadDB,
        WriteDB,
        EditDB
    };

    public class Role {

        private static Dictionary<Roles, List<Permissions>> PermissionsForRoles = null;

        static Role()
        {
            PermissionsForRoles = LoadPermissionsFromFile(Config.PermissionsConfigPath);
        }

        public Role(Roles cr) {
            currentRole = cr;
            GrantedPermissions = PermissionsForRoles[currentRole];
        }

        public Roles currentRole { get; set; }
        public List<Permissions> GrantedPermissions { get; set; }

        private static Dictionary<Roles, List<Permissions>> LoadPermissionsFromFile(string filePath)
        {
            var returnValue = new Dictionary<Roles, List<Permissions>>();

            string textFromFile = "FILE_ERROR";

            using (StreamReader sr = new StreamReader(Config.PermissionsConfigPath))
            {
                textFromFile = sr.ReadToEnd();
            }

            //ako je doslo do greske prilikom citanja fajla onda ce promenljiva textFromFile ostati ista
            if (textFromFile.Equals("FILE_ERROR"))
            {
                Console.WriteLine("Config fajl za permisije nije ucitan. Ne moze da se garantuje rad programa! Ocekivana lokacija configa: " + filePath);
            }

            List<string> TextLinesFromFile = textFromFile.Split('\n').ToList();
            //prodjemo kroz svaki red, parsiramo ga i na osnovu toga roli dodamo dozvolu
            for(int i = 0; i < TextLinesFromFile.Count; i++)
            {
                //preskace se prazan red
                if (TextLinesFromFile[i].Trim().Length == 0)
                {
                    continue;
                }

                //jedan red treba da izgleda ovako: Admin,ReadDB
                //ovde proverava da li ima dva elementa
                string[] parsedData = TextLinesFromFile[i].Trim().Split(',');
                if (parsedData.Length != 2)
                {
                    Console.WriteLine("Invalid permission rule");
                    continue;
                }

                Roles tmpRole = RoleFromString(parsedData[0]);
                Permissions tmpPerm = PermissionFromString(parsedData[1]);

                if (returnValue.ContainsKey(tmpRole))
                {
                    returnValue[tmpRole].Add(tmpPerm);
                } else
                {
                    returnValue.Add(tmpRole, new List<Permissions>());
                    returnValue[tmpRole].Add(tmpPerm);
                }
            }


            return returnValue;
        }

        private static Roles RoleFromString(string txt)
        {
            var tmp = txt.Trim().ToLower();
            if (tmp == "admin")
            {
                return Roles.Admin;
            } else if (tmp == "reader")
            {
                return Roles.Reader;
            } else if (tmp == "writer")
            {
                return Roles.Writer;
            }
            throw new InvalidDataException("Role parsing error");
        }

        private static Permissions PermissionFromString(string txt)
        {
            var tmp = txt.Trim().ToLower();
            if (tmp == "createdb")
            {
                return Permissions.CreateDB;
            }
            else if (tmp == "deletedb")
            {
                return Permissions.DeleteDB;
            }
            else if (tmp == "readdb")
            {
                return Permissions.ReadDB;
            }
            else if (tmp == "writedb")
            {
                return Permissions.WriteDB;
            }
            else if (tmp == "editdb")
            {
                return Permissions.EditDB;
            }
            throw new InvalidDataException("Permission parsing error");
        }
    }

}
