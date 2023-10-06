using crypto;
using Google.Protobuf.WellKnownTypes;
using Markdig.Helpers;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Net.Mime.MediaTypeNames;

namespace NOGAste
{
    public static class StringExtractionTools
    {


        public static Dictionary<string, string> convertMsgToFields(string message)
        {
            //string cleanStr = Regex.Replace(message, @"\\", "\\");
            Dictionary<string, string> msgDict = new Dictionary<string, string>();
            List<string> msgFieldList = new List<string>();
            string[] words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string part1 = "";
            string part2 = "";
            string curVal = "";
            int numRecords = 0;
            int numAcctNameFail    = 0;
            int numAcctNameSuccess = 0;
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Parsing Message: {message}");
            Console.WriteLine("------------------------------------------------");
            for (int i = 0; i < words.Length; i++)
            {

                //Console.WriteLine("------------------------------------------------");
                Console.WriteLine($"Current Word: {words[i]}");
                Console.WriteLine("------------------------------------------------");

                //Subject/1 appears to never have anything in it
                //else if (words[i] == "Subject:")
                //{
                //    Console.WriteLine($"Matched Subject:");
                //    Console.ReadLine();
                //    msgFieldList.Add(words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]);
                //    msgDict.Add("Subject", words[i + 1]);
                //    Console.WriteLine($">>>Added Subject: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                //    i++;
                //    
                //}//Subject



                //EventMSG 1a
                //Console.WriteLine($"----BEGIN MATCHING Current Word i:{i}, WrdLen:{words.Length}, TTL Words: {message.Length}, Searching for: >{words[i]}< --------");
                //Special case of starting at index [0]
                if (words[0] == "The" || words[0] == "Offline")
                {   //When "EventMsg:" is missing from the EvenMsg, we add it, so this check must always be after chk for "EventMsg" so we don't duplicate
                    //Console.WriteLine($"Matched HEADLESS EventMsg");
                    //Console.ReadLine();
                    string tmpStr2 = "";
                    //i++;

                    //int j = 0; //in case there is no "." we need to stop the train
                    while (i < words.Length)
                    {
                        if (words[i].Contains('.'))
                        {
                            tmpStr2 += words[i]; // Include the word with the period
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Last Word:{words[i]}");
                            break; // Exit the loop when a period is encountered
                        }
                        else
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]}");
                            tmpStr2 += words[i] + " "; // Concatenate words with spaces
                        }

                        i++;

                        // Check if we've reached the end of the array
                        if (i >= words.Length)
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]} REACHED THE END");
                            break;
                        }
                    }

                    // Add the constructed sentence to the dictionary
                    if (msgDict.TryGetValue("EventMsg", out curVal))
                    {
                        msgDict["EventMsg"] = curVal + tmpStr2;
                    }
                    else
                    {
                        msgDict.Add("EventMsg", tmpStr2);
                    }

                    if ( i+2 <= words.Length  && words[i+1] == "The") { i++; }  //If there are two sentences, skip the second one

                    Console.WriteLine($"EXITING The/Offline i:{i}, WordLen:{words.Length}");
                    Console.WriteLine($">>>Added EventMsg: {tmpStr2}");
                }//EventMsg "The"





                //EventMSG 1b
                else if (words[i] == "EventMsg")
                {   //sometimes it appears as just "EventMsg" without the ":"
                    //Console.WriteLine($"Matched EventMsg");
                    //Console.ReadLine();
                    string tmpStr1 = words[i];
                    string tmpStr2 = "";
                    i++; //advance past the "EventMsg"



                    while (i < words.Length)
                    {
                        if (words[i].Contains('.'))
                        {
                            tmpStr2 += words[i]; // Include the word with the period
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Last Word:{words[i]}");
                            break; // Exit the loop when a period is encountered
                        }
                        else
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]}");
                            tmpStr2 += words[i] + " "; // Concatenate words with spaces
                        }

                        i++;

                        // Check if we've reached the end of the array
                        if (i >= words.Length)
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]} REACHED THE END");
                            break;
                        }
                    }


                    if (msgDict.TryGetValue(("EventMsg"), out curVal))
                    {
                        msgDict["EventMsg"] = curVal + tmpStr2;
                    }
                    else
                    {
                        msgDict.Add("EventMsg", tmpStr2);
                    }

                    Console.WriteLine($">>>Added EventMsg: {tmpStr1 + " " + tmpStr2}"); ;
                    //i++;
                }//EventMsg:





                //EventMSG 1c
                else if (words[i] == "Successfully")
                {   //When "EventMsg:" is missing from the EvenMsg, we add it, so this check must always be after chk for "EventMsg" so we don't duplicate
                    //Console.WriteLine($"Matched Successfully");
                    //Console.ReadLine();
                    string tmpStr1 = words[i];
                    string tmpStr2 = "";


                    while (i < words.Length)
                    {
                        if (words[i].Contains('.'))
                        {
                            tmpStr2 += words[i]; // Include the word with the period
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Last Word:{words[i]}");
                            break; // Exit the loop when a period is encountered
                        }
                        else
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]}");
                            tmpStr2 += words[i] + " "; // Concatenate words with spaces
                        }

                        i++;

                        // Check if we've reached the end of the array
                        if (i >= words.Length)
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]} REACHED THE END");
                            break;
                        }
                    }


                    Console.WriteLine($">>>Added EventMsg: {tmpStr1 + " " + tmpStr2}");
                    //i++;
                }//EventMsg "Succesfully"





                //Security ID/2
                else if (words[i] == "Security" && words[i + 1] == "ID:" && words[i + 2] != "Account")
                {
                    //Console.WriteLine($"Matched Security ID:");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2] );
                    string secIDUpd = words[i + 2] + " " + words[i + 3];

                    if (msgDict.TryGetValue(("SecurityID"), out curVal))
                    {
                        msgDict["SecurityID"] = curVal + secIDUpd;
                    }
                    else
                    {
                        msgDict.Add("SecurityID", words[i + 2] );
                    }
                    Console.WriteLine($">>>Added  SecurityID: {words[i] + " " + words[i + 1] + " " + words[i + 2] }");
                    //Console.ReadLine();
                    i += 2;
                }//Security ID:









                //Account Name/3
                else if (words[i] == "Account" && words[i + 1] == "Name:"  &&  (words[i + 2] != "New" || words[i + 2] != "Account")   ) 
                {
                    //Console.WriteLine($"Matched Account Name:");
                    //Console.ReadLine();
                    string acctNamUpd = words[i + 2];

                    try
                    {
                        curVal = "";
                        if (!msgDict.ContainsKey("UserID") )
                        { 
                                msgDict.Add("UserID", words[i + 2]);
                                Console.WriteLine($"Field Name Valid:{words[i + 2]}");
                                Console.WriteLine($"Words:{message}");
                                //Console.ReadLine();
                        }

                        Console.WriteLine($">>>Added UserID: {words[i + 2]}");
                        i += 2;
                    }
                    catch
                    {
                        //Console.WriteLine($"Field: {i} Failed to read Account Name.....");

                        //Console.ReadLine();
                        Console.WriteLine($"Words:{words}");
                        numAcctNameFail++;
                    }//end catch

                    
                    Console.WriteLine($"Event Logs Attempted: {numRecords}, UserID Success:{numAcctNameSuccess}, UserID Fail:{numAcctNameFail} ");

                }//Account Name:/3







                //Account Domain/4
                else if (words[i] == "Account" && words[i + 1] == "Domain:" && ( words[i + 2].Contains("NT") || words[i + 2] == "WORKGROUP" )  )
                {
                    //Console.WriteLine($"Matched Account Domain:");
                    //Console.ReadLine();

                    //msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2]);

                    string acctDomUpd = "";


                    if (msgDict.TryGetValue(("AcctDomain"), out  curVal))
                    {
                        msgDict["AcctDomain"] = curVal + acctDomUpd;
                    }
                    else
                    {
                        msgDict.Add("AcctDomain", words[i + 2]);
                    }

                        Console.WriteLine($">>>Added AcctDomain: {acctDomUpd}");

                    //Console.ReadLine();
                    i += 2;
                }//Account Domain:/4






                //This is not a column - but this info appears here more reliably
                //so I am adding it here
                //MachineName/5
                else if (words[i].Substring(0, words[i].Length - 1) == "MachineName")
                {   //is MachineName:blahblah  no space after ":"
                    int index1 = words[i].IndexOf(":");
                    //Console.WriteLine($"Matched MachineName");
                    //Console.ReadLine();

                    part1 = words[i].Substring(0, index1 - 1); //MachineName
                    part2 = words[i].Substring(index1 + 1, words[i].Length - 1); //blahblahblah  .Length - index1

                    msgFieldList.Add(part1 + " " + part2);
                    msgDict.Add("MachineName", part2);
                    Console.WriteLine($">>>Added MachineName: {part1 + " " + part2}");
                }//MachineName:/5






                //Logon ID/6
                else if ( words.Length - i > 3 )
                {
                    if (words[i] == "Logon" && words[i + 1] == "ID:" && words[i + 2].Contains("0x"))
                    {
                        try
                        {
                            //Console.WriteLine($"Matched Logon ID:");
                            //Console.ReadLine();
                            msgDict.Add("LogonID", words[i + 2]);
                            Console.WriteLine($">>>Added LogonID: {words[i + 2]}");
                            //Console.ReadLine();
                            i += 2;

                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine($"Field: {i}, Failed Read Logon ID");
                            //Console.ReadLine();
                        }//catch
                    }//inner if
                }//Logon ID/6






                //Logon Type/7
                else if (words[i] == "Logon" && words[i + 1] == "Type:"  && words[i + 2] != "Account")
                {
                    //Console.WriteLine($"Matched Logon Type:");
                    //Console.ReadLine();
                    msgDict.Add("LogonType: ", words[i + 2]);
                    Console.WriteLine($">>>Added LogonType: {words[i + 2]}");
                    //Console.ReadLine();
                    i += 2;
                }//Logon Type:/7



                //Logon Type/8
                else if (words[i] == "Elevated" && words[i + 1] == "Token:" && ( words[i + 2] == "Yes" || words[i + 2] == "No" ))
                {
                    //Console.WriteLine($"Matched Logon Type:");
                    //Console.ReadLine();
                    msgDict.Add("Elevated_Token: ", words[i + 2]);
                    Console.WriteLine($">>>Added ElevToken: {words[i + 2]}");
                    //Console.ReadLine();
                    i += 2;
                }//Logon Type:/8



                //Logon Type/9
                else if (words[i] == "Impersonation" && words[i + 1] == "Level:" && words[i + 2] == "Impersonation")
                {
                    //Console.WriteLine($"Matched Logon Type:");
                    //Console.ReadLine();
                    msgDict.Add("ImpersonationLvl: ", words[i + 2]);
                    Console.WriteLine($">>>Added ImpersonationLvl: {words[i + 2]}");
                    //Console.ReadLine();
                    i += 2;
                }//Logon Type:/9





                //Logon Failed/8
                //This really is a header - but contains the keyword "Failed"
                //So I'm creating my own header/info
                //Account Logon Failure/6
                else if (words[i] == "Account" && words[i + 1] == "For" && words[i + 2] == "Which" && words[i + 3] == "Logon" && words[i + 4] == "Failed:")
                {
                    //Console.WriteLine($"Matched Account Logon Failure:");
                    //Console.ReadLine();

                    string acctFailUpd = "";
                    //bool incExtra = true;

                        //This is always the case - so remove this
                        //if (words[i + 5] == "Security")
                        //{
                        //    acctFailUpd = "NULL";
                        //    incExtra = false;
                        //}
                    //if
                    //{
                    //    acctFailUpd = "Logon Failed";
                    //    //incExtra = true;
                    //}


                    if (msgDict.TryGetValue(("LogonFailure"), out curVal))
                    {
                        msgDict["LogonFailure"] = curVal + acctFailUpd;
                    }
                    else
                    {
                        msgDict.Add("LogonFailure", acctFailUpd);
                    }
                    //if (incExtra)
                    //{
                    //    //Console.WriteLine($">>>Added  Account Logon Failure: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + acctFailUpd}");
                    //    i += 5;
                    //}
                    //else
                    //{

                    Console.WriteLine($">>>Added  LogonFailure: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + acctFailUpd}");

                    i += 4;
                    //}
                    //Console.ReadLine();
                }//Account For Which Logon Failed/8








                //New Process Name/9
                else if (words[i] == "New" && words[i + 1] == "Process" && words[i + 2] == "Name:" && words[i + 2].Contains("C:\\") )
                {
                    //Console.WriteLine($"Matched New Process Name:");
                    //Console.ReadLine();
                    msgDict.Add("NewProcessName", words[i + 3]);
                    Console.WriteLine($">>>Added NewProcessName: {words[i + 2] + " " + words[i + 3]}");
                    i += 3;
                }//New Process Name:/9







                //Process Command/10
                else if (words[i] == "Process" && words[i + 1] == "Command") // && words[i + 2] == "Line:")
                {
                    try
                    {
                        //Console.WriteLine($"Matched Process Command Line:");
                        //Console.ReadLine();
                        msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]);
                        msgDict.Add("ProcessCmdLine", words[i + 3]);
                        //Console.WriteLine($">>>Added Process Command Line: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]}");
                        Console.WriteLine($">>>Added ProcessCmdLine: {words[i + 3]}");
                        i += 3;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Field Fail: ProcessCmdLine......");
                    }

                    //Console.ReadLine();
                }//Processs Command Line:/10








                //Failure Information/11
                else if (words[i] == "Failure" && words[i + 1] == "Information:"  && words[i + 2] != "Failure")
                {
                    //Console.WriteLine($"Matched Failure Information:");
                    //Console.ReadLine();
                    string tmpStr5 = words[i + 2];



                    if (msgDict.TryGetValue(("FailInfo"), out curVal))
                    {
                        msgDict["FailInfo"] = curVal + words[i + 2];
                    }
                    else
                    {
                        msgDict.Add("FailInfo", words[i + 2]);
                    }

                    Console.WriteLine($">>>Added FailInfo: {words[i + 2]}");
                    //Console.ReadLine();
                    i += 2;
                }//Failure Information:/11







                //Failure Reason/12
                else if (words[i] == "Failure" && words[i + 1] == "Reason:"  && words[i + 2] == "Unknown")
                {
                    //Console.WriteLine($"Matched Failure Reason:");
                    //Console.ReadLine();
                    //Console.WriteLine($"Failure Reason BEFORE INC, i:{i}, WrdLen:{words.Length}, Current Word:{words[i]}");
                    i+=2; //skip over "Reason:"
                    //Console.WriteLine($"Failure Reason AFTER INC, BEFORE LOOP i:{i}, WrdLen:{words.Length}, Current Word:{words[i]}");
                    string tmpStr5 = "";

                    while (i < words.Length)
                    {
                        if (words[i].Contains('.'))
                        {
                            tmpStr5 += words[i]; // Include the word with the period
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Last Word:{words[i]}");
                            break; // Exit the loop when a period is encountered
                        }
                        else
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]}");
                            tmpStr5 += words[i] + " "; // Concatenate words with spaces
                        }

                        i++;

                        // Check if we've reached the end of the array
                        if (i >= words.Length)
                        {
                            //Console.WriteLine($"i:{i}, WrdLen:{words.Length}, Current Word:{words[i]} REACHED THE END");
                            break;
                        }
                    }


                    if (msgDict.TryGetValue(("FailReason"), out curVal))
                    {
                        msgDict["FailReason"] = curVal + tmpStr5;
                    }
                    else
                    {
                        msgDict.Add("FailReason", tmpStr5);
                    }

                    Console.WriteLine($">>>Added FailReason: {tmpStr5}");
                    //Console.ReadLine();

                }//Failure Reason:/12











                //Status/13
                else if (words[i] == "Status:" && words[i + 1].Contains("0x"))
                {
                    Console.WriteLine($"Matched Status");
                    //Console.ReadLine();
                    msgDict.Add("Status", words[i + 1]);
                    Console.WriteLine($">>>Added Status: {words[i + 1]}");
                    i++;
                }//Status:/13





                //Sub Status/14
                else if (words[i] == "Sub" && words[i + 1] == "Status:" && words[i + 2].Contains("0x"))
                {
                    //Console.WriteLine($"Matched SubStatus");
                    //Console.ReadLine();
                    msgDict.Add("Sub_Status", words[i + 2]);
                    Console.WriteLine($">>>Added SubStatus: {words[i + 2]}");
                    i += 2;
                }//Sub Status:/14





                //Process Information/15
                else if (words[i] == "Process" && words[i + 1] == "Information:" && words[i + 2].Contains("C:\\"))
                {
                    //Console.WriteLine($"Matched Process Information");
                    //Console.ReadLine();
                    msgDict.Add("ProcessInfo", words[i + 2]);
                    Console.WriteLine($">>>Added ProcessInfo: {words[i + 2]}");
                    i+=2;
                }//Sub Status:15





                //Process Information/16
                else if (words[i] == "Object" && words[i + 1] == "Name:" && words[i + 2].Contains("C:\\"))
                {
                    //Console.WriteLine($"Matched Process Information");
                    //Console.ReadLine();
                    msgDict.Add("ObjectName", words[i + 2]);
                    Console.WriteLine($">>>Added ObjectName:  {words[i + 2]}");
                    i +=2;
                }//Sub Status:16





                //Reason/17
                else if (words[i] == "Reason:" && words[i + 1] == "RulesEngine.")
                {
                    //Console.WriteLine($"Matched Reason:");
                    //Console.ReadLine();
                    msgDict.Add("Reason", words[i + 1]);
                    Console.WriteLine($">>>AddedReason: {words[i + 1]}");
                    //Console.ReadLine() ;
                    i++;
                }//Reason:/17





                //else if (words[i] == "ID:" && i < message.Length)
                //{
                //    //Console.WriteLine($"Matched ID:");
                //    //Console.ReadLine();
                //    //msgFieldList.Add(words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]);

                //    try
                //    {
                //        string tmpStr6 = "";
                //        tmpStr6 = words[i + 1];

                //        if (msgDict.TryGetValue(("ID"), out string updatedStr))
                //        {
                //            string tmpStr3 = msgDict["ID"] = msgDict["ID"] + updatedStr;
                //        }
                //        else
                //        {
                //            msgDict.Add("ID", words[i + 1]);
                //        }
                //        //Console.WriteLine($">>>Added Matched ID: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                //        //Console.ReadLine();
                //        i++;
                //    }//try
                //    catch (Exception e)
                //    {
                //        i = i;
                //    }
                //}//ID:




                //else
                //{
                //    //Console.WriteLine($"NO SPECIFIC MATCH Adding: {words[i]}");
                //    msgFieldList.Add(words[i]);
                //    msgDict.Add("Unknown", ","+words[i]);
                //}

                //Console.WriteLine($"----String Extraction From MSG Fields END -------");
                numRecords++;
            }//for

            Console.WriteLine($"Event Recovered From CSV\n");
            Console.WriteLine($"Event Logs Attempted: {numRecords}, UserID Success:{numAcctNameSuccess}, UserID Fail:{numAcctNameFail} ");
            //Console.ReadLine();
            //for (int i = 0; i < msgDict.Count; i++)
            //{
            //    Console.WriteLine($"Key: {msgDict.ElementAt(i).Key},  Value: {msgDict.ElementAt(i).Value} ");
            //}
            //Console.WriteLine($"Finished Processing Message List - a Look at msgDict before we return ");
            //Console.ReadLine();
            //Console.ReadLine();
            //return msgFieldList;
            return msgDict;
        }//convertMsgToFields







        public static string ExtractFromListReturnSubStr(List<string> msgFieldList, string targetString)
        {
            int index1 = 0;
            string fieldStr = "";

            //1st, get the "field" out of the list
            for (int i = 0; i < msgFieldList.Count; i++)
            {
                if (msgFieldList[i] == targetString)
                {
                    fieldStr = msgFieldList[i];
                }

            }


            //2nd, get the substring we are after
            index1 = fieldStr.IndexOf(targetString);
            if (index1 < index1 - targetString.Length)
            {
                return fieldStr.Substring(index1 + targetString.Length, index1 - targetString.Length).Trim();
            }
            else
            {
                return "NOT FOUND";
            }
        }//ExtractFromListReturnSubStr





        public static string ExtractFieldDelimBoundary(string message, string targetString, string boundaryString)
        {
            int index1 = 0;
            int index2 = 0;

            message.IndexOf(targetString);
            index1 = message.IndexOf(targetString);
            index2 = message.IndexOf(boundaryString);
            if (index1 != -1 && index2 - index1 - targetString.Length > 0 && index2 - index1 - 14! > message.Length)
            {
                return (message.Substring(index1 + targetString.Length, index2 - index1 - targetString.Length)).Trim();
            }
            else
            {
                return "BLANK";
            }
        }//ExtractFieldDelimBoundary




    }//class
}//Namespace
