using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Module9.SecurityThreatsSO.Message;

namespace Module9
{
    public class ExcelImporter
    {
        public static List<Tuple<TypeText, TypeInfo, TypePlace, TypePlane, string, string>> RawData;
        public static bool LoadRawData(string path, string file)
        {
            RawData = ReadFromXLSXFile(path, file);

            return RawData != null;
        }
        public static List<Tuple<TypeText, TypeInfo, TypePlace, TypePlane, string, string>> ReadFromXLSXFile(string path, string file)
        {
            string filePath = path + file;

            List<Tuple<TypeText, TypeInfo, TypePlace, TypePlane, string, string>> resultList = new List<Tuple<TypeText, TypeInfo, TypePlace, TypePlane, string, string>>();

            if (!CheckPathExist(path, file)) return null;

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // Skipping first row
                            if (reader.Depth.Equals(0)) continue;

                            // If row is empty continue reading workbook, if row has empty data break reading workbook
                            CheckRow(reader, out bool identifierIsEmpty, out bool rowIsNotEmpty);
                            if (identifierIsEmpty && !rowIsNotEmpty)
                            {
                                Debug.Log($"Row {reader.Depth + 1} in {reader.Name} is empty ");
                                continue;
                            }
                            else if (!identifierIsEmpty && !rowIsNotEmpty)
                            {
                                Debug.LogError($"Row {reader.Depth + 1} in {reader.Name} has empty data");
                                break;
                            }
                            else if (identifierIsEmpty && rowIsNotEmpty)
                            {
                                Debug.LogError($"Row {reader.Depth + 1} in {reader.Name} has empty identifier");
                                break;
                            }
                            List<string> rowData = new List<string>();

                            for (int i = 1; i < 4; i++)
                            {
                                // If in row is empty cells break reading row
                                if (RowHasEmptyCell(reader, i)) break;
                                // Get cell values
                                string cellValue = reader.GetString(i);
                                rowData.Add(cellValue);
                            }
                            Tuple<TypeText, TypeInfo,TypePlace, TypePlane, string, string> tuple = new(DetectTextType(rowData[1]), DetectInfoType(rowData[1]), DetectPlaceType(rowData[1]), DetectPlaneType(rowData[1]), rowData[0], rowData[2]);
                            resultList.Add(tuple);
                        }
                    } while (reader.NextResult()); // Go to the next workbook
                }
            }

            return resultList;
        }
        private static TypeText DetectTextType(string value)
        {
            return value switch
            {
                "E" => TypeText.Entry,
                "P2" => TypeText.Passengers2,
                "P1" => TypeText.Passengers1,
                "P12" => TypeText.Passengers12,
                "F1" => TypeText.Fuel1,
                "F2" => TypeText.Fuel2,
                "F12" => TypeText.Fuel12,
                "M" => TypeText.Materials,
                "W" => TypeText.Wind,
                "AL" => TypeText.Alarm,
                _ => TypeText.None
            };
        }
        private static TypePlane DetectPlaneType(string value)
        {
            return value switch
            {
                "V1" => TypePlane.CRJ900,
                "V2" => TypePlane.A320Neo,
                "V3" => TypePlane.B738,
                "V12" => TypePlane.A320NeoAndCRJ900,
                _ => TypePlane.None
            };
        }
        private static TypeInfo DetectInfoType(string value)
        {
            return value switch
            {
                "IF" => TypeInfo.InformationFire,
                "IB" => TypeInfo.InformationBrokenLanding,
                "IM" => TypeInfo.InformationMedical,
                "IL" => TypeInfo.InformationLanding,
                "ICR" => TypeInfo.InformationCrash,
                "IS" => TypeInfo.InformationSecurity,
                "IC" => TypeInfo.InformationUnderConstruction,
                _ => TypeInfo.None
            };
        }
        private static TypePlace DetectPlaceType(string value)
        {
            return value switch
            {
                "A" => TypePlace.Approach,
                "AP" => TypePlace.Apron,
                _ => TypePlace.None
            };
        }
        private static bool CheckPathExist(string path, string file)
        {
            string filePath = path + file;
            bool pathExist = true;

            if (Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Debug.Log($"Path {filePath} exist");

                if (File.Exists(filePath))
                    Debug.Log($"File {file} exist");
                else
                {
                    Debug.Log($"File {file} not exist");
                    pathExist = false;
                }
            }
            else
            {
                Debug.Log($"Path {filePath} not exist");
                pathExist = false;
            }

            return pathExist;
        }
        private static void CheckRow(IExcelDataReader reader, out bool identifierIsEmpty, out bool rowIsNotEmpty, int columnValue = 3)
        {
            rowIsNotEmpty = false;

            if (reader.GetValue(0) == null)
            {
                for (int i = 1; i < columnValue; i++)
                {
                    if (reader.IsDBNull(i)) continue;
                    rowIsNotEmpty = true;
                }

                identifierIsEmpty = true;
            }
            else
            {
                for (int i = 1; i < columnValue; i++)
                {
                    if (reader.IsDBNull(i)) continue;
                    rowIsNotEmpty = true;
                }

                identifierIsEmpty = false;
            }
        }
        private static bool RowHasEmptyCell(IExcelDataReader reader, int columnValue)
        {
            bool rowHasEmptyCell = false;

            for (int i = 0; i < columnValue; i++)
            {
                if (!reader.IsDBNull(i)) continue;
                rowHasEmptyCell = true;
            }

            if (rowHasEmptyCell)
                Debug.LogError($"Row {reader.Depth + 1} in {reader.Name} has empty cell, fill in cell in this row");

            return rowHasEmptyCell;
        }
    }
}