using System.Text.RegularExpressions; //To support Regex(Regular Expression) function.
using System.Collections.Generic; //For dictionaly. 
using System.Linq; //For checking all list in array are contains something for one time.
using System; //System basic library.

namespace StringManageClass.Utility
{
    //To create the conditons that have for 3 status.
    public enum CorrectionStatus
    {
        towards,
        Incorrect,
        Unknown
    }
    //Suporting dictionary
    public class RecordAllData
    {
        //? is the Num can be adding the Null.
        public int? Num { get; set; } 
        public int? Top { get; set; }
        public int? Bot { get; set; }
        public int? Tode { get; set; }
        public DateTime RecordDate { get; set; } 
    }
    public class stringManage
    {
        // Pre condition for holding the value in this class.
        private string _inputVal;
        private string _EmpthyStr = "  ";
        private List<string> _StrArr = new List<string>(); //Dynamic array to hold splited string.
        //Pattern to find "โ" follow by "ด" or "ต" with or without tone mark.
        private string _pattern = @"โ[้๊่]?[ดต]";
        //Pattern to find only "บ", "ล", or "ต"
        private string _pattern_b = @"[บ]";
        private string _pattern_l = @"[ล]";
        private string _pattern_t = @"[ต]";
        //Saperate between number and character.
        private string _pattern_num = @"[1234567890]";
        private string _Special_char = @"[-+*xX]";
        //private string _Special_eql = @"[=]";
        private string _pattern_blt = @"[่โบลต]";
        private int _pos = 0;
        public static string Name_key = string.Empty;
        private char[] _Delimiters = { '-', '+', '*', 'x', 'X', '=','×'};
        // Declare for static if we want to acress from anywhere.
        public static Dictionary<string, List<RecordAllData>> DataCollection = new Dictionary<string, List<RecordAllData>>();


        // classes have one or more constructors
        public stringManage(string RawInput)
        {
            _inputVal = SpaceAdjustment(RawInput);
            _StrArr = _inputVal.Split(' ').ToList(); //Split string to array when found the ' '.
            StringArragement(); //Formating the values to easy management.
            PutNametoTop(); //Changing sequence of name to top of array.
            FormatingStr(); //Formating the value to YY=YYxYYxYY
            AddingToDictionaryAndDisplay();
        }
        //To manage space in valiable. Every space will be only 1 time space.
        public string SpaceAdjustment(string input)
        {
            input = input.Trim(); //Remove space at first and last.
            input = input.Replace(".", "").Replace(",", ""); //Remove . and , in string.
            //Loop to replace the space to only 1 time space.
            for (int CountSpace = 0; CountSpace <= 6; CountSpace++)
            {
                if (input.Contains(_EmpthyStr))
                {
                    input = input.Replace(_EmpthyStr, " ");
                }
            }
            int NumStr = input.Length; //Count number of character in string.
            for (int CountStr = 0; CountStr <= NumStr - 1; CountStr++)
            {
                //Interesting spacial characters to cover all of conditions. Might be more!
                if (input[CountStr] == '=' || input[CountStr] == '-' ||
                    input[CountStr] == '_' || input[CountStr] == '+' ||
                    input[CountStr] == '*' || input[CountStr] == '|' ||
                    input[CountStr] == '#' || input[CountStr] == ':' ||
                    input[CountStr] == ';' || input[CountStr] == '<' || input[CountStr] == '>')
                {
                    if (input[CountStr - 1] == ' ')
                    {
                        input = input.Remove(CountStr - 1, 1); //Remove space before spacial character.
                        NumStr = input.Length; //Recount number of character in string.
                        CountStr--; //Compensate the CountStr after remove 1 character.
                    }
                    if (input[CountStr + 1] == ' ')
                    {
                        input = input.Remove(CountStr + 1, 1); //Remove space after spacial character.
                        NumStr = input.Length; //Recount number of character in string.
                    }
                }
                //input = input.Replace(input[CountStr].ToString(), " ");
            }
            return input;
        }
        public void StringArragement()
        {
            int NumVal = _StrArr.Count(); //Count number of string in array.
            for (int i = 0; i < NumVal; i++)
            {
                int StrArrLen = _StrArr[i].Length; //Count number of character in string.
                bool isNum_first = char.IsDigit(_StrArr[i][0]); //Check if first character is number.
                bool isNum_last = char.IsDigit(_StrArr[i][StrArrLen - 1]); //Check if last character is number.
                if (isNum_first == false)
                {
                    if (isNum_last == true)
                    {
                        //Split character and number.
                        int index = Regex.Match(_StrArr[i], _pattern_num).Index;
                        //Get the string from index number 0 to index of first number.
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                    }
                }
                else
                {
                    if (isNum_last == false)
                    {
                        //Split number and character.
                        int index = Regex.Match(_StrArr[i], _pattern_blt).Index;
                        //Get the string from index number 0 to index of last number.
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                    }
                }
                if (_StrArr[i].Contains("บน") && _StrArr[i].Contains("ล่าง"))
                {
                    if (_StrArr[i].IndexOf("บน") < _StrArr[i].IndexOf("ล่าง"))
                    {
                        //split "บน" and "ล่าง" to make 2 strings.
                        int index = _StrArr[i].IndexOf("ล่าง");
                        //Get the string from index number 0 to index of "ล่าง".
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                        // NumVal++; //Compensate the NumVal after insert new string.
                        // continue; //Skip to next loop.
                    }
                    else
                    {
                        //Do the same as above but split "ล่าง" first.
                        int index = _StrArr[i].IndexOf("บน");
                        //Get the string from index number 0 to index of "บน".
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                        // NumVal++; //Compensate the NumVal after insert new string.
                        // continue; //Skip to next loop.
                    }
                }
                else if (_StrArr[i].Contains("บน") && Regex.IsMatch(_StrArr[i], _pattern))
                {
                    if (_StrArr[i].IndexOf("บน") < Regex.Match(_StrArr[i], _pattern).Index)
                    {
                        //split "บน" and "โ" follow by "ด" or "ต" to make 2 strings.
                        int index = Regex.Match(_StrArr[i], _pattern).Index;
                        //Get the string from index number 0 to index of "โ" follow by "ด" or "ต".
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                        // NumVal++; //Compensate the NumVal after insert new string.
                        // continue; //Skip to next loop.
                    }
                    else
                    {
                        //Do the same as above but split "โ" follow by "ด" or "ต" first.
                        int index = _StrArr[i].IndexOf("บน");
                        //Get the string from index number 0 to index of "บน".
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                        // NumVal++; //Compensate the NumVal after insert new string.
                        // continue; //Skip to next loop.
                    }
                }
                else if (_StrArr[i].Contains("ล่าง") && Regex.IsMatch(_StrArr[i], _pattern))
                {
                    if (_StrArr[i].IndexOf("ล่าง") < Regex.Match(_StrArr[i], _pattern).Index)
                    {
                        //split "ล่าง" and "โ" follow by "ด" or "ต" to make 2 strings.
                        int index = Regex.Match(_StrArr[i], _pattern).Index;
                        //Get the string from index number 0 to index of "โ" follow by "ด" or "ต".
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                        // NumVal++; //Compensate the NumVal after insert new string.
                        // continue; //Skip to next loop.
                    }
                    else
                    {
                        //Do the same as above but split "โ" follow by "ด" or "ต" first.
                        int index = _StrArr[i].IndexOf("ล่าง");
                        //Get the string from index number 0 to index of "ล่าง".
                        string Str1 = _StrArr[i].Substring(0, index).Trim();
                        string Str2 = _StrArr[i].Substring(index).Trim();
                        _StrArr[i] = Str1;
                        _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                        // NumVal++; //Compensate the NumVal after insert new string.
                        // continue; //Skip to next loop.
                    }
                }
                // To throw out the other cases for meeting the condition.
                if (StrArrLen <= 3)
                {
                    //Check for ("บ" and "ล") and ("ล" and "บ") in the string.
                    if (StrArrLen == 2)
                    {
                        if (Regex.IsMatch(_StrArr[i], _pattern_b) && Regex.IsMatch(_StrArr[i], _pattern_l))
                        {
                            string Str1 = _StrArr[i][0].ToString();
                            string Str2 = _StrArr[i][1].ToString();
                            _StrArr[i] = Str1;
                            _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                            // NumVal++; //Compensate the NumVal after insert new string.
                            // continue; //Skip to next loop.
                        }
                    }
                    else if (StrArrLen == 3)
                    {
                        if (Regex.IsMatch(_StrArr[i], _pattern_b) &&
                            Regex.IsMatch(_StrArr[i], _pattern_l) &&
                            Regex.IsMatch(_StrArr[i], _pattern_t))
                        {
                            string Str1 = _StrArr[i][0].ToString();
                            string Str2 = _StrArr[i][1].ToString();
                            string Str3 = _StrArr[i][2].ToString();
                            _StrArr[i] = Str1;
                            _StrArr.Insert(i + 1, Str2); //Insert new string to next index.
                            _StrArr.Insert(i + 2, Str3); //Insert new string to next index.
                            // NumVal += 2; //Compensate the NumVal after insert new strings.
                            // continue; //Skip to next loop.
                        }
                    }
                }
            }
            //Pre condition.
            CorrectionStatus status = CorrectionStatus.Unknown;
            int poscount = 0; //To count number of position to insert.
            bool first = false; //To mark the first time to set _pos.
            bool LastStatus = false; //To remember the last status of incorrect.
            bool isNum = false; //To check current string is number or not.
            bool isNum_next = false; //To check next string is number or not.
            _pos = 0; //Reset _pos before use.
            bool Allcondition = false; //To check current string meet all conditions or not.
            bool Allcondition_next = false; //To check next string meet all conditions or not.
            bool FirstNum = false; //For checking if the first value in array is number.

            // Second Loop to correct the incorrect arrangement.
            for (int i = 0; i < NumVal; i++)
            {
                isNum = char.IsDigit(_StrArr[i][0]); //Check if first character is number.
                //Just added condition to check all conditions.
                Allcondition = (_StrArr[i] == "บน" || _StrArr[i] == "ล่าง" ||
                                Regex.IsMatch(_StrArr[i], _pattern) || _StrArr[i] == "บ" ||
                                _StrArr[i] == "ล" || _StrArr[i] == "ต");
                if (i < NumVal - 1)
                {
                    isNum_next = char.IsDigit(_StrArr[i + 1][0]); //Check if first character of next string is number.
                    Allcondition_next = (_StrArr[i + 1] == "บน" || _StrArr[i + 1] == "ล่าง" ||
                                         Regex.IsMatch(_StrArr[i + 1], _pattern) || _StrArr[i + 1] == "บ" ||
                                         _StrArr[i + 1] == "ล" || _StrArr[i + 1] == "ต");
                }
                //To check if the first value in array is number.
                if (char.IsDigit(_StrArr[0][0]))
                {
                    FirstNum = true;
                }
                // To check if it is the last string in array.
                if (i == NumVal - 1)
                {
                    // Final check to correct the incorrect arrangement.
                    if (status == CorrectionStatus.Incorrect)
                    {
                        _StrArr.Insert(_pos + poscount, _StrArr[i]);
                        _StrArr.RemoveAt(i + 1);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                if (status == CorrectionStatus.towards)
                {
                    if (isNum_next)
                    {
                        continue;
                    }
                    else if (Allcondition_next)
                    {
                        //To check if last status is incorrect or first value in array is number.
                        if (LastStatus || FirstNum)
                        {
                            status = CorrectionStatus.Incorrect;
                            continue;
                        }
                        else
                        {
                            status = CorrectionStatus.Unknown;
                            continue;
                        }
                    }
                }
                if (status == CorrectionStatus.Incorrect)
                {
                    if (Allcondition_next)
                    {
                        _StrArr.Insert(_pos + poscount, _StrArr[i]);
                        _StrArr.RemoveAt(i + 1);
                        poscount++;
                        continue;
                    }
                    else if (isNum_next)
                    {
                        _StrArr.Insert(_pos + poscount, _StrArr[i]);
                        _StrArr.RemoveAt(i + 1);
                        poscount = 0;
                        first = false;
                        status = CorrectionStatus.Unknown;
                        LastStatus = true;
                        continue;
                    }
                    else if (!isNum_next && !Allcondition_next)
                    {
                        _StrArr.Insert(_pos + poscount, _StrArr[i]);
                        _StrArr.RemoveAt(i + 1);
                        break;
                    }
                }
                if (Allcondition)
                {
                    if (isNum_next)
                    {
                        status = CorrectionStatus.towards;
                    }
                    else if (Allcondition_next)
                    {
                        status = CorrectionStatus.Unknown;
                    }
                }
                else if (isNum)
                {
                    if (isNum_next)
                    {
                        status = CorrectionStatus.towards;
                        if (first == false)
                        {
                            _pos = i;
                            first = true;
                        }
                    }
                    else if (Allcondition_next)
                    {
                        status = CorrectionStatus.Incorrect;
                        if (first == false)
                        {
                            _pos = i;
                            first = true;
                        }
                    }
                }
            }
        }
        public void PutNametoTop()
        {
            int NumVal = _StrArr.Count(); //Count number of string in array.
            bool isNum = false; //To check current string is number or not.
            bool Allcondition = false; //To check current string meet all conditions or not.
            int count = 0; //To count the value that not conditions and number.

            for (int i = 0; i < NumVal; i++)
            {
                isNum = char.IsDigit(_StrArr[i][0]); //Check if first character is number.
                Allcondition = (_StrArr[i] == "บน" || _StrArr[i] == "ล่าง" ||
                                Regex.IsMatch(_StrArr[i], _pattern) || _StrArr[i] == "บ" ||
                                _StrArr[i] == "ล" || _StrArr[i] == "ต");
                if (isNum == false && Allcondition == false)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            _StrArr.Insert(0, _StrArr[i]);
                            _StrArr.RemoveAt(i + 1);
                            count += 1;
                        }
                        else
                        {
                            _StrArr[0] = _StrArr[0] + ' ' + _StrArr[i];
                            _StrArr.RemoveAt(i);
                            i -= 1;
                            NumVal -= 1;
                        }
                    }
                }
            }
        }
        public void FormatingStr()
        {
            bool IsThereEuqual = _StrArr.Any(s => s.Contains("=")); //checking the "=" in one time.
            bool IsThereSpecialChar = _StrArr.Any(s => Regex.IsMatch(s, _Special_char)); //checking the special char in one time.
            bool isNum = false; //To check current string is number or not.
            bool Allcondition = false; //To check current string meet all conditions or not.
            bool Allcondition_next = false; //To check next string meet all conditions or not.
            bool FirstOfnum = false;
            bool st = false;
            int count_blt = 0; //To count when allcondition == true.
            int PosNum = 0; //Position that interested.
            int counting = 0; //Just count to match with PosNum.
            if (IsThereEuqual == false)
            {
                int NumVal = _StrArr.Count(); //Count number of string in array.
                for (int i = 1; i < NumVal; i++) //i=1 because avioding the name of client.
                {
                    isNum = char.IsDigit(_StrArr[i][0]); //Check if first character is number.
                    Allcondition = (_StrArr[i] == "บน" || _StrArr[i] == "ล่าง" ||
                                    Regex.IsMatch(_StrArr[i], _pattern) || _StrArr[i] == "บ" ||
                                    _StrArr[i] == "ล" || _StrArr[i] == "ต");
                    if (i < NumVal - 1)
                    {
                        Allcondition_next = (_StrArr[i + 1] == "บน" || _StrArr[i + 1] == "ล่าง" ||
                                        Regex.IsMatch(_StrArr[i + 1], _pattern) || _StrArr[i + 1] == "บ" ||
                                        _StrArr[i + 1] == "ล" || _StrArr[i + 1] == "ต");
                    }
                    if (IsThereSpecialChar == false)
                    {
                        if (Allcondition)
                        {
                            count_blt += 1; //Count for collecting the number of allcondition to perform formating correctly.
                        }
                        if (isNum)
                        {
                            if (FirstOfnum == false) //Check if the first number that found before condition.
                            {
                                FirstOfnum = true;
                                PosNum = i;
                            }
                            else
                            {
                                // Formating the value to YY=YYxYYxYY.
                                if (st == false)
                                {
                                    _StrArr[PosNum] = _StrArr[PosNum] + "=" + _StrArr[i];
                                    _StrArr.RemoveAt(i);
                                    i -= 1;
                                    NumVal -= 1;
                                    st = true;
                                    counting += 1;
                                }
                                else
                                {
                                    _StrArr[PosNum] = _StrArr[PosNum] + 'x' + _StrArr[i];
                                    _StrArr.RemoveAt(i);
                                    i -= 1;
                                    NumVal -= 1;
                                    counting += 1;
                                }

                                if (counting == count_blt)
                                {
                                    FirstOfnum = false;
                                    st = false;
                                    counting = 0;
                                }
                            }
                            if (Allcondition_next)
                            {
                                count_blt = 0;
                            }
                        }
                    }
                    else //Do the same above but if there are special char, Just add "=".
                    {
                        if (isNum)
                        {
                            if (FirstOfnum == false)
                            {
                                FirstOfnum = true;
                                PosNum = i;
                            }
                            else
                            {
                                _StrArr[PosNum] = _StrArr[PosNum] + "=" + _StrArr[i];
                                _StrArr.RemoveAt(i);
                                i -= 1;
                                NumVal -= 1;
                                FirstOfnum = false;
                            }
                        }
                    }

                }
            }
        }
        public void AddingToDictionaryAndDisplay()
        {
            bool Allcondition = false; //To check current string meet all conditions or not.
            bool Allcondition_next = false; //To check next string meet all conditions or not.
            bool isNum = false; //To check current string is number or not.
            int NumVal = _StrArr.Count(); //Count number of string in array.
            int counting = 0;
            List<string> CondArr = new List<string>(); //Dynamic array to collect the conditions.
            List<string> Values = new List<string>();
            for (int i = 0; i < NumVal; i++)
            {
                isNum = char.IsDigit(_StrArr[i][0]); //Check if first character is number.
                Allcondition = (_StrArr[i] == "บน" || _StrArr[i] == "ล่าง" ||
                                Regex.IsMatch(_StrArr[i], _pattern) || _StrArr[i] == "บ" ||
                                _StrArr[i] == "ล" || _StrArr[i] == "ต");
                if (i < NumVal - 1)
                {
                    Allcondition_next = (_StrArr[i + 1] == "บน" || _StrArr[i + 1] == "ล่าง" ||
                                         Regex.IsMatch(_StrArr[i + 1], _pattern) || _StrArr[i + 1] == "บ" ||
                                         _StrArr[i + 1] == "ล" || _StrArr[i + 1] == "ต");
                }
                if (i == 0)
                {
                    Name_key = _StrArr[0];
                    DataCollection.Add(Name_key, new List<RecordAllData>()); //Set Key of dic.
                    continue;
                }
                else if (Allcondition)
                {
                    if (_StrArr[i] == "บน" || _StrArr[i] == "บ")
                    {
                        CondArr.Add(_StrArr[i]);
                    }
                    else if (_StrArr[i] == "ล่าง" || _StrArr[i] == "ล")
                    {
                        CondArr.Add(_StrArr[i]);
                    }
                    else
                    {
                        CondArr.Add(_StrArr[i]);
                    }
                    counting += 1;
                }
                else if (isNum) //Format should be "Number, Top, Bottom, Tod"
                {
                    var Recording = new RecordAllData();
                    Recording.RecordDate = DateTime.Now; // Set DateTime.
                    Values = _StrArr[i].Split(_Delimiters).ToList();
                    for (int ii = 0; ii < 4; ii++)
                    {
                        if (ii < CondArr.Count+1)
                        {
                            if (ii == 0)
                            {
                                Recording.Num = int.Parse(Values[ii]);
                            }
                            else if (CondArr[ii - 1] == "บน" || CondArr[ii - 1] == "บ")
                            {
                                Recording.Top = int.Parse(Values[ii]);
                            }
                            else if (CondArr[ii - 1] == "ล่าง" || CondArr[ii - 1] == "ล")
                            {
                                Recording.Bot = int.Parse(Values[ii]);
                            }
                            else
                            {
                                Recording.Tode = int.Parse(Values[ii]);
                            }
                        }
                    }
                    DataCollection[Name_key].Add(Recording);
                    List<RecordAllData> dataToShow = DataCollection[Name_key];

                    if (Allcondition_next) //Reset counting value to initial.
                    {
                        counting = 0;
                        CondArr.Clear();
                    }
                }
            }
        }
    }
}