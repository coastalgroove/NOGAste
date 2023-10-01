using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NOGAste
{
    public static class StringExtractionTools
    {


        public static List<string> convertMsgToFields(string message)
        {
            //string cleanStr = Regex.Replace(message, @"\\", "\\");
            Dictionary<string, string> msgDict = new Dictionary<string, string>();
            List<string> msgFieldList = new List<string>();
            string[] words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string part1 = "";
            string part2 = "";


            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine($"----BEGIN MATCHING Current Word i:{i}, WrdLen:{words.Length}, TTL Words: {message.Length}, Searching for: >{words[i]}< --------");
                if (words[0] == "The" || words[0] == "Offline")
                {   //When "EventMsg:" is missing from the EvenMsg, we add it, so this check must always be after chk for "EventMsg" so we don't duplicate
                    Console.WriteLine($"Matched HEADLESS EventMsg");
                    //Console.ReadLine();
                    string tmpStr1 = words[0];
                    string tmpStr2 = "";
                    i++;
                    //int j = 0; //in case there is no "." we need to stop the train
                    while (i < words.Length - i && !words[i].Contains('.'))
                    {
                        tmpStr2 += words[i] + " ";
                        if (i < words.Length - 1) { i++; }
                        //Console.WriteLine($"Searching for .  {tmpStr2} ");
                    }
                    //Get the last word
                    if (i < words.Length - 1) { tmpStr2 += " " + words[i]; }


                    if (msgDict.TryGetValue(("EventMsg"), out string curVal))
                    {
                        msgDict["EventMsg"] = curVal + tmpStr2;
                    }
                    else
                    {
                        msgDict.Add("EventMsg", tmpStr2);
                    }

                    msgFieldList.Add("EventMsg: " + " " + tmpStr1 + " " + tmpStr2);
                    Console.WriteLine($"Added EventMsg: {"EventMsg:" + " " + tmpStr1 + " " + tmpStr2}");
                    //i++;
                }//EventMsg "The"






                else if (words[i] == "EventMsg")
                {   //sometimes it appears as just "EventMsg" without the ":"
                    Console.WriteLine($"Matched EventMsg");
                    //Console.ReadLine();
                    string tmpStr1 = words[i];
                    string tmpStr2 = "";
                    i++;
                    while (!words[i].Contains('.'))
                    {
                        tmpStr2 += words[i] + " ";
                        i++;
                        Console.WriteLine($"Searching for .  {tmpStr2} ");
                    }
                    //get last word
                    tmpStr2 += " " + words[i];

                    if (msgDict.TryGetValue(("EventMsg"), out string curVal))
                    {
                        msgDict["EventMsg"] = curVal + tmpStr2;
                    }
                    else
                    {
                        msgDict.Add("EventMsg", tmpStr2);
                    }

                    msgFieldList.Add("EventMsg: " + " " + tmpStr1 + " " + tmpStr2);
                    Console.WriteLine($"Added EventMsg: {"EventMsg:" + " " + tmpStr1 + " " + tmpStr2}"); ;
                    //i++;
                }//EventMsg:




                else if (words[i] == "Successfully")
                {   //When "EventMsg:" is missing from the EvenMsg, we add it, so this check must always be after chk for "EventMsg" so we don't duplicate
                    Console.WriteLine($"Matched Successfully");
                    //Console.ReadLine();
                    string tmpStr1 = words[i];
                    string tmpStr2 = "";
                    i++;
                    while (!words[i].Contains('.'))
                    {
                        tmpStr2 += words[i] + " ";
                        i++;
                        //Console.WriteLine($"Searching for .  {tmpStr2} ");
                    }
                    //Get the last word
                    tmpStr2 += " " + words[i];

                    if (msgDict.TryGetValue(("EventMsg"), out string curVal))
                    {
                        msgDict["EventMsg"] = curVal + tmpStr2;
                    }
                    else
                    {
                        msgDict.Add("EventMsg", tmpStr2);
                    }

                    msgFieldList.Add("EventMsg: " + " " + tmpStr1 + " " + tmpStr2);
                    Console.WriteLine($"Added EventMsg: {"EventMsg:" + " " + tmpStr1 + " " + tmpStr2}");
                    //i++;
                }//EventMsg "Succesfully"



















                else if (words[i] == "Account" && words[i + 1] == "Name:")
                {
                    Console.WriteLine($"Matched Account Name:");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2]);

                    string acctNamUpd = words[i + 2];

                    if (msgDict.TryGetValue(("Security ID"), out string curVal))
                    {
                        msgDict["Account Name"] = curVal + acctNamUpd;
                    }
                    else
                    {
                        msgDict.Add("Account Name", words[i + 2]);
                    }

                    Console.WriteLine($"Added Account Name: {words[i] + " " + words[i + 1] + " " + words[i + 2]}");
                    i += 2;
                }//Account Name:






                else if (words[i] == "New" && words[i + 1] == "Process" && words[i + 2] == "Name:")
                {
                    Console.WriteLine($"Matched New Process Name:");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]);
                    msgDict.Add("New Process Name", words[i + 3]);
                    Console.WriteLine($"Added Process Name: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]}");
                    i += 3;
                }//New Process Name:



                else if (words[i] == "Process" && words[i + 1] == "Command") // && words[i + 2] == "Line:")
                {
                    try
                    {
                        Console.WriteLine($"Matched Process Command Line:");
                        //Console.ReadLine();
                        msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]);
                        msgDict.Add("Process Command Line", words[i + 3]);
                        Console.WriteLine($"Added Process Command Line: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]}");
                        i += 3;
                    }
                    catch (Exception e)
                    {
                        i += 2;
                    }
                }//Processs Command Line:



                else if (words[i] == "Reason:")
                {
                    Console.WriteLine($"Matched Reason:");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1]);
                    msgDict.Add("Reason", words[i + 1]);
                    Console.WriteLine($"Added Reason: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                    //Console.ReadLine() ;
                    i++;
                }//Reason:


                //else if (words[i] == "Subject:")
                //{
                //    Console.WriteLine($"Matched Subject:");
                //    Console.ReadLine();
                //    msgFieldList.Add(words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]);
                //    msgDict.Add("Subject", words[i + 1]);
                //    Console.WriteLine($"Added Subject: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                //    i++;
                //    
                //}//Subject



                else if (words[i] == "Security" && words[i + 1] == "ID:")
                {
                    Console.WriteLine($"Matched Security ID:");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]);
                    string secIDUpd = words[i + 2] + " " + words[i + 3];

                    if (msgDict.TryGetValue(("Security ID"), out string curVal))
                    {
                        msgDict["Security ID"] = curVal + secIDUpd;
                    }
                    else
                    {
                        msgDict.Add("Security ID", words[i + 2] + " " + words[i + 3]);
                    }
                    Console.WriteLine($"Added  Security ID: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]}");
                    //Console.ReadLine();
                    i += 3;
                }//Security ID:



                else if (words[i] == "Account" && words[i + 1] == "For" && words[i + 2] == "Which" && words[i + 3] == "Logon" && words[i + 4] == "Failed:")
                {
                    Console.WriteLine($"Matched Account Logon Failure:");
                    //Console.ReadLine();

                    string acctFailUpd = "";
                    bool incExtra = true;

                    if (words[i + 5] == "Security")
                    {
                        acctFailUpd = "NULL";
                        incExtra = false;
                    }
                    else
                    {
                        acctFailUpd = words[i + 5];
                        incExtra = true;
                    }


                    if (msgDict.TryGetValue(("Account Logon Failure"), out string curVal))
                    {
                        msgDict["Account Logon Failure"] = curVal + acctFailUpd;
                    }
                    else
                    {
                        msgDict.Add("Account Logon Failure", acctFailUpd);
                    }
                    if (incExtra)
                    {
                        Console.WriteLine($"Added  Account Logon Failure: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + acctFailUpd}");
                        i += 5;
                    }
                    else
                    {
                        Console.WriteLine($"Added  Account Logon Failure: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + acctFailUpd}");

                        i += 4;
                    }
                    //Console.ReadLine();
                }//Account For Which Logon Failed





                else if (words[i] == "Logon" && words[i + 1] == "ID:")
                {
                    try
                    {

                        Console.WriteLine($"Matched Logon ID:");
                        //Console.ReadLine();
                        msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2]);
                        msgDict.Add("Logon ID", words[i + 2]);
                        Console.WriteLine($"Added Logon ID: {words[i] + " " + words[i + 1] + " " + words[i + 2]}");
                        //Console.ReadLine();
                        i += 2;
                    }
                    catch (Exception e)
                    {
                        i += 1;
                    }
                }//Logon ID




                //else if (words[i] == "SID" && words[i + 1] == "Account" && words[i + 1] == "Name")
                //{
                //    Console.WriteLine($"Matched SID Account Name:");
                //    Console.ReadLine();
                //    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]);
                //    msgDict.Add("SID Account", words[i + 2]);
                //    Console.WriteLine($"Added SID Account Name: {words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3]}");
                //    Console.ReadLine();
                //    i += 3;
                //}//SID



                else if (words[i] == "Logon" && words[i + 1] == "Type:")
                {
                    Console.WriteLine($"Matched Logon Type:");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2]);
                    msgDict.Add("Logon Type: ", words[i + 2]);
                    Console.WriteLine($"Added Logon Type: {words[i] + " " + words[i + 1] + " " + words[i + 2]}");
                    //Console.ReadLine();
                    i += 2;
                }//Logon Type:




                else if (words[i] == "Failure" && words[i + 1] == "Information:")
                {
                    Console.WriteLine($"Matched Failure Information:");
                    //Console.ReadLine();
                    string tmpStr4 = "";

                    i++;
                    while (!words[i].Contains('.'))
                    {
                        tmpStr4 += words[i] + " ";
                        i++;
                        //Console.WriteLine($"Searching for .  {tmpStr4} ");
                    }
                    //Get the last word
                    tmpStr4 += words[i] + " ";
                    string failInfoUpd = tmpStr4;

                    if (msgDict.TryGetValue(("Failure Information"), out string curVal))
                    {
                        msgDict["Failure Information"] = curVal + failInfoUpd;
                    }
                    else
                    {
                        msgDict.Add("Failure Information", failInfoUpd);
                    }

                    msgDict.Add("Failure Information: ", failInfoUpd);
                    Console.WriteLine($"Failure Information: {failInfoUpd}");
                    //Console.ReadLine();
                    i += 2;
                }//Failure Information:






                else if (words[i] == "Failure" && words[i + 1] == "Reason:")
                {
                    Console.WriteLine($"Matched Failure Reason:");
                    //Console.ReadLine();
                    string tmpStr5 = "";

                    i++;
                    while (!words[i].Contains('.'))
                    {
                        tmpStr5 += words[i] + " ";
                        i++;
                        //Console.WriteLine($"Searching for .  {tmpStr5} ");
                    }
                    //Get the last word
                    tmpStr5 += words[i] + " ";
                    string failReasonUpd = tmpStr5;


                    if (msgDict.TryGetValue(("Failure Reason"), out string curVal))
                    {
                        msgDict["Failure Reason"] = curVal + failReasonUpd;
                    }
                    else
                    {
                        msgDict.Add("Failure Reason", failReasonUpd);
                    }

                    msgDict.Add("Failure Reason: ", failReasonUpd);
                    Console.WriteLine($"Failure Reason: {failReasonUpd}");
                    //Console.ReadLine();
                    i += 2;
                }//Failure Reason:







                else if (words[i] == "Account" && words[i + 1] == "Domain:")
                {
                    Console.WriteLine($"Matched Account Domain:");
                    //Console.ReadLine();




                    //msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2]);



                    string acctDomUpd = "";
                    bool incExtra = true;

                    if (words[i + 2] == "Failure")
                    {
                        acctDomUpd = "NULL";
                        incExtra = false;
                    }
                    else
                    {
                        acctDomUpd = words[i + 2];
                        incExtra = true;
                    }



                    if (msgDict.TryGetValue(("Account Domain"), out string curVal))
                    {
                        msgDict["Account Domain"] = curVal + acctDomUpd;
                    }
                    else
                    {
                        msgDict.Add("Account Domain", words[i + 2]);
                    }

                    if (incExtra)
                    {
                        Console.WriteLine($"Added Account Domain: {words[i] + " " + words[i + 1] + acctDomUpd}");
                        i++;
                    }
                    else
                    {
                        Console.WriteLine($"Added Account Domain: {words[i] + " " + words[i + 1] + acctDomUpd}");
                    }
                    //Console.ReadLine();
                    i += 2;
                }//Account Domain:




                else if (words[i] == "ID:" && i < message.Length)
                {
                    Console.WriteLine($"Matched ID:");
                    //Console.ReadLine();
                    //msgFieldList.Add(words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]);

                    try
                    {
                        string tmpStr6 = "";
                        tmpStr6 = words[i + 1];

                        if (msgDict.TryGetValue(("ID"), out string updatedStr))
                        {
                            string tmpStr3 = msgDict["ID"] = msgDict["ID"] + updatedStr;
                        }
                        else
                        {
                            msgDict.Add("ID", words[i + 1]);
                        }
                        Console.WriteLine($"Added Matched ID: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                        //Console.ReadLine();
                        i++;
                    }//try
                    catch (Exception e)
                    {
                        i = i;
                    }
                }//ID:



                else if (words[i] == "Status:" && words[i + 1] == "0x" && words[i + 1] != "Sub")
                {
                    Console.WriteLine($"Matched Status");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1]);
                    msgDict.Add("Status", words[i + 1]);
                    Console.WriteLine($"Added Status: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                    i++;
                }//Status:


                else if (words[i] == "Sub" && words[i + 1] == "Status:")
                {
                    Console.WriteLine($"Matched Sub Status");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1] + " " + words[i + 2]);
                    msgDict.Add("Sub Status", words[i + 2]);
                    Console.WriteLine($"Added Sub Status: {words[i] + " " + words[i + 1] + " " + words[i + 2]}");
                    i += 2;
                }//Sub Status:



                else if (words[i] == "Process" && words[i + 1] == "Information:")
                {
                    Console.WriteLine($"Matched Process Information");
                    //Console.ReadLine();
                    msgFieldList.Add(words[i] + " " + words[i + 1]);
                    msgDict.Add("Process Information", words[i + 1]);
                    Console.WriteLine($"Added Process Information: {words[i].Substring(0, words[i].Length - 1) + " " + words[i + 1]}");
                    i++;
                }//Sub Status:



                else if (words[i].Substring(0, words[i].Length - 1) == "MachineName")
                {   //is MachineName:blahblah  no space after ":"
                    int index1 = words[i].IndexOf(":");
                    Console.WriteLine($"Matched MachineName");
                    //Console.ReadLine();

                    part1 = words[i].Substring(0, index1 - 1); //MachineName
                    part2 = words[i].Substring(index1 + 1, words[i].Length - 1); //blahblahblah  .Length - index1

                    msgFieldList.Add(part1 + " " + part2);
                    msgDict.Add("MachineName", part2);
                    Console.WriteLine($"Added MachineName: {part1 + " " + part2}");
                }//MachineName:








                else
                {
                    Console.WriteLine($"NO SPECIFIC MATCH Adding: {words[i]}");
                    msgFieldList.Add(words[i]);

                }

                Console.WriteLine($"----END MATCHING -------");
            }//for
            //Console.WriteLine($"Finished Processing Message List");
            //Console.ReadLine();
            return msgFieldList;

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
