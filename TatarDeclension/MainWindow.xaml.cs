using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TatarDeclension
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char _lastLetter;
        private char[] _voicelessedConsonants = {'п', 'т', 'к', 'ф', 'ч', 'ш', 'щ', 'с', 'х', 'һ', 'ц', 'ъ', 'ь'};

        private readonly char[] _allConsonants =
        {
            'б', 'в', 'г', 'д', 'ж', 'җ', 'з', 'л', 'м', 'н', 'ң', 'р', 'й', 'п', 'т', 'к', 'ф', 'ч', 'ш', 'щ', 'с',
            'х', 'һ', 'ц', 'ъ', 'ь'
        };

        private bool _isSoft;
        private bool _containsOnlyChameleonVowels;

        private readonly char[] _vowels = {'ә', 'ө', 'э', 'а', 'о', 'ы', 'е', 'ё', 'я'};

        private readonly char[] _consonants =
        {
            'б', 'в', 'г', 'д', 'ж', 'җ', 'з', 'л', 'м', 'н', 'ң', 'р', 'с', 'т', 'ф', 'х', 'һ', 'ц', 'ч', 'ш', 'щ', 'ъ'
        };

        private const char FirstException = 'п';
        private const char SecondException = 'к';
        private const char ThirdException = 'й';
        private const char FourthException = 'и';
        private readonly string[] _people = {"әни", "әби", "әти", "бәби"};
        private const char FifthException = 'ь';
        private const char SixthException = 'ю';
        private const char SeventhException = 'у';
        private readonly char[] _softVowels = {'ә', 'ө', 'ү', 'э', 'и'};
        private readonly char[] _hardVowels = {'а', 'о', 'у', 'ы', 'ё'};
        private readonly char[] _chameleonVowels = {'е', 'ю', 'я'};
        private string _word;
        private string[] _possessedForms;
        private readonly char[] _lastException = {'м', 'н', 'ң'};

        private readonly char[] _sonorantedConsonants =
            {'б', 'в', 'г', 'д', 'ж', 'җ', 'з', 'л', 'м', 'н', 'ң', 'р', 'й'};

        private readonly char[] _allVowels = {'ә', 'ө', 'и', 'ү', 'э', 'а', 'о', 'у', 'ы', 'е', 'ю', 'я'};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Declense_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = Input.Text.ToLower().TrimStart(' ').TrimEnd(' ');
            _word = Input.Text;
            if (_word == "")
            {
                MessageBox.Show("Пожалуйста, введите слово, которое вы хотите просклонять");
                return;
            }

            try
            {
                //слово подчиняется закону сингармонизма
                if (IsInVowelHarmony.IsChecked == true)
                {
                    //проверка слова на мягкость
                    _isSoft = CheckIfIsSoft();
                    //добавление аффиксов принадлежности
                    _possessedForms = PossessedForms();
                    _lastLetter = _word.Last();
                    //добавление аффиксов падежей
                    VowelHarmonyDeclension();
                    return;
                }

                //слово не подчиняется закону сингармонизма
                ArabicAndPersianDeclension();
            }
            catch
            {
                MessageBox.Show("Вы ввели некорректное слово. Пожалуйста, исправьте ошибку.");
            }
        }

        private bool CheckIfIsSoft()
        {
            if (CheckIfInRoundingHarmony())
            {
                return true;
            }

            return _softVowels.Contains(GetLastVowel()) || _containsOnlyChameleonVowels;
        }

        private bool CheckIfInRoundingHarmony()
        {
            bool first = false, second = false;
            foreach (char letter in _word)
            {
                if (first && letter == 'а')
                {
                    return true;
                }

                if (second && letter == 'ә')
                {
                    return true;
                }

                switch (letter)
                {
                    case 'ә':
                        first = true;
                        continue;
                    case 'а':
                        second = true;
                        break;
                    default: continue;
                }
            }

            return false;
        }

        private char GetLastVowel()
        {
            List<char> vowels = new List<char>();
            string buffer = "";
            char lastVowel;
            _containsOnlyChameleonVowels = CheckIfContainsOnlyChameleonVowels(ref vowels);
            if (_containsOnlyChameleonVowels)
            {
                lastVowel = vowels.Last();
            }
            else
            {
                while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()) && _word.Length > 1)
                {
                    buffer += _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                lastVowel = _word.Last();

                _word += Reverse(buffer);
                if (_chameleonVowels.Contains(lastVowel))
                {
                    GetLastVowel();
                }
            }

            return lastVowel;
        }

        private bool CheckIfContainsOnlyChameleonVowels(ref List<char> vowels)
        {
            foreach (char letter in _word)
            {
                if (_allVowels.Contains(letter))
                {
                    vowels.Add(letter);
                }
            }

            return vowels.All(vowel => !_hardVowels.Contains(vowel) && !_softVowels.Contains(vowel));
        }

        private string[] PossessedForms()
        {
            string[] possessedForms = new string[6];
            char lastLetter = _word.Last();
            if (_vowels.Contains(lastLetter))
            {
                possessedForms[0] = _word + 'м';
                possessedForms[2] = _word + 'ң';
                if (!_isSoft)
                {
                    possessedForms[1] = _word + "быз";
                    possessedForms[3] = _word + "гыз";
                    possessedForms[4] = _word + "сы";
                    possessedForms[5] = _word + "лары";
                }
                else
                {
                    possessedForms[1] = _word + "без";
                    possessedForms[3] = _word + "гез";
                    possessedForms[4] = _word + "се";
                    possessedForms[5] = _word + "ләре";
                }

                return possessedForms;
            }

            if (_consonants.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    PossessHard(possessedForms);
                }
                else
                {
                    PossessSoft(possessedForms);
                }

                return possessedForms;
            }

            switch (lastLetter)
            {
                case FirstException:
                {
                    //замена п на б
                    return PossessException('б', possessedForms);
                }
                case SecondException:
                {
                    //замена к на г
                    return PossessException('г', possessedForms);
                }
                case ThirdException:
                {
                    //замена й на е
                    _word = _word.Remove(_word.Length - 1);
                    _word += 'е';
                    possessedForms[0] = _word + 'м';
                    possessedForms[2] = _word + 'ң';
                    if (!_isSoft)
                    {
                        possessedForms[1] = _word + "быз";
                        possessedForms[3] = _word + "гыз";
                        possessedForms[5] = _word + "лары";
                    }
                    else
                    {
                        possessedForms[1] = _word + "без";
                        possessedForms[3] = _word + "гез";
                        possessedForms[5] = _word + "ләре";
                    }

                    possessedForms[4] = _word;
                    return possessedForms;
                }
                case FourthException:
                {
                    PossessSoft(possessedForms);
                    return possessedForms;
                }
                case FifthException:
                {
                    //удаление ь
                    _word = _word.Remove(_word.Length - 1);
                    _isSoft = true;
                    PossessSoft(possessedForms);
                    return possessedForms;
                }
                case SixthException:
                {
                    return PossessException(' ', possessedForms);
                }
                case SeventhException:
                {
                    if (_vowels.Contains(_word[_word.Length - 2]))
                    {
                        //замена у на в
                        _word = _word.Remove(_word.Length - 1);
                        _word += 'в';
                    }

                    PossessHard(possessedForms);
                    return possessedForms;
                }
                default:
                {
                    if (_vowels.Contains(_word[_word.Length - 2]))
                    {
                        //замена ү на в
                        _word = _word.Remove(_word.Length - 1);
                        _word += 'в';
                    }

                    PossessSoft(possessedForms);
                    return possessedForms;
                }
            }
        }

        private string[] PossessException(char consonantedLetter, string[] possessedForms)
        {
            if (consonantedLetter != ' ')
            {
                _word = _word.Remove(_word.Length - 1);
                _word += consonantedLetter;
            }

            if (!_isSoft)
            {
                PossessHard(possessedForms);
            }
            else
            {
                PossessSoft(possessedForms);
            }

            return possessedForms;
        }

        private void PossessHard(IList<string> possessedForms)
        {
            possessedForms[0] = _word + "ым";
            possessedForms[1] = _word + "ыбыз";
            possessedForms[2] = _word + "ың";
            possessedForms[3] = _word + "ыгыз";
            possessedForms[4] = _word + 'ы';
            possessedForms[5] = _word + "лары";
        }

        private void PossessSoft(IList<string> possessedForms)
        {
            possessedForms[0] = _word + "ем";
            possessedForms[1] = _word + "ебез";
            possessedForms[2] = _word + "ең";
            possessedForms[3] = _word + "егез";
            if (_people.Contains(_word))
            {
                possessedForms[4] = _word + "се";
                possessedForms[5] = _word + "ләре";
            }
            else
            {
                possessedForms[4] = _word + 'е';
                possessedForms[5] = _word + "ләре";
            }
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private string BasePlural()
        {
            _lastLetter = _word[_word.Length - 1];
            if (_lastException.Contains(_lastLetter))
            {
                return _isSoft ? _word + "нәр" : _word + "нар";
            }

            return _isSoft ? _word + "ләр" : _word + "лар";
        }

        private void VowelHarmonyDeclension()
        {
            List<Row> rows = new List<Row>(28);
            rows.Add(new Row { Singular = "Первое лицо"});
            BaseFill(rows, _possessedForms[0], _possessedForms[1]);
            rows.Add(new Row { Singular = "Второе лицо"});
            BaseFill(rows, _possessedForms[2], _possessedForms[3]);
            rows.Add(new Row { Singular = "Третье лицо"});
            BaseFill(rows, _possessedForms[4], _possessedForms[5]);
            rows.Add(new Row { Singular = "Основа + аффикс падежа"});
            _word = Input.Text;
            BaseFill(rows, _word, BasePlural());
            PossessiveCaseDeclension(rows);
            DativeCaseDeclension(rows);
            AccusativeCaseDeclension(rows);
            AblativeCaseDeclension(rows);
            LocativeCaseDeclension(rows);

            // TODO: временное решение, переносит «простое» в начало. Потом нужно сделать, чтобы сразу «простое» было в начале
            List<T> ShiftLeft<T>(List<T> list, int shiftBy)
            {
                if (list.Count <= shiftBy)
                {
                    return list;
                }

                List<T> result = list.GetRange(shiftBy, list.Count - shiftBy);
                result.AddRange(list.GetRange(0, shiftBy));
                return result;
            }

            rows = ShiftLeft(rows, 21);

            Table.ItemsSource = rows;
        }

        private static void BaseFill(ICollection<Row> rows, string singular, string plural)
        {
            rows.Add(new Row {Case = "Баш килеш", Singular = singular, Plural = plural});
            rows.Add(new Row {Case = "Иялек килеше", Singular = singular, Plural = plural});
            rows.Add(new Row {Case = "Юнәлеш килеше", Singular = singular, Plural = plural});
            rows.Add(new Row {Case = "Төшем килеше", Singular = singular, Plural = plural});
            rows.Add(new Row {Case = "Чыгыш килеше", Singular = singular, Plural = plural});
            rows.Add(new Row {Case = "Урын-вакыт килеше", Singular = singular, Plural = plural});
        }

        // Иялек килеше
        private void PossessiveCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = {2, 9, 16, 23};
            string[] affixes = {"ның", "нең", "ның", "нең"};
            GeneralisationForPossessiveAndAccusativeCases(rows, indices, affixes);
        }

        private void GeneralisationForPossessiveAndAccusativeCases(IReadOnlyList<Row> rows, IReadOnlyList<int> indices,
            IReadOnlyList<string> affixes)
        {
            //единственное число, первое и второе лица
            _word = rows[indices[0]].Singular;
            if (!_isSoft)
            {
                rows[indices[0]].Singular += affixes[0];
                rows[indices[1]].Singular += affixes[0];
            }
            else
            {
                rows[indices[0]].Singular += affixes[1];
                rows[indices[1]].Singular += affixes[1];
            }

            //множественное число, первое и второе лица
            _word = rows[indices[0]].Plural;
            if (!_isSoft)
            {
                rows[indices[0]].Plural += affixes[0];
                rows[indices[1]].Plural += affixes[0];
            }
            else
            {
                rows[indices[0]].Plural += affixes[1];
                rows[indices[1]].Plural += affixes[1];
            }

            //единственное и множественное числа, третье лицо
            _word = rows[indices[2]].Singular;
            if (!_isSoft)
            {
                rows[indices[2]].Singular += affixes[2];
                rows[indices[2]].Plural += affixes[2];
            }
            else
            {
                rows[indices[2]].Singular += affixes[3];
                rows[indices[2]].Plural += affixes[3];
            }

            //основа + аффикс падежа
            _word = Input.Text;

            if (!_isSoft)
            {
                rows[indices[3]].Singular += affixes[0];
                rows[indices[3]].Plural += affixes[0];
            }
            else
            {
                rows[indices[3]].Singular += affixes[1];
                rows[indices[3]].Plural += affixes[1];
            }
        }

        // Юнәлеш килеше
        private void DativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = {3, 10, 17, 24};
            string[] affixes =
                {"а", "ә", "га", "гә", "на", "нә", "ка", "кә", "ка", "кә", "на", "нә", "га", "гә", "ка", "кә"};
            GeneralisationForDativeAndLocativeCases(rows, indices, affixes);
        }

        private void GeneralisationForDativeAndLocativeCases(IReadOnlyList<Row> rows, IReadOnlyList<int> indices,
            IReadOnlyList<string> affixes)
        {
            //единственное число, первое и второе лица
            _word = rows[indices[0]].Singular;
            char lastLetter = _word.Last();
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[indices[0]].Singular += affixes[0];
                    rows[indices[1]].Singular += affixes[0];
                }
                else
                {
                    rows[indices[0]].Singular += affixes[1];
                    rows[indices[1]].Singular += affixes[1];
                }

                //множественное число, первое и второе лица
                _word = rows[indices[0]].Plural;
                if (!_isSoft)
                {
                    rows[indices[0]].Plural += affixes[2];
                    rows[indices[1]].Plural += affixes[2];
                }
                else
                {
                    rows[indices[0]].Plural += affixes[3];
                    rows[indices[1]].Plural += affixes[3];
                }

                //единственное и множественное числа, третье лицо
                _word = rows[indices[2]].Singular;
                if (!_isSoft)
                {
                    rows[indices[2]].Singular += affixes[4];
                    rows[indices[2]].Plural += affixes[4];
                }
                else
                {
                    rows[indices[2]].Singular += affixes[5];
                    rows[indices[2]].Plural += affixes[5];
                }
            }
            else
            {
                if (!_isSoft)
                {
                    rows[indices[0]].Singular += affixes[6];
                    rows[indices[1]].Singular += affixes[6];
                }
                else
                {
                    rows[indices[0]].Singular += affixes[7];
                    rows[indices[1]].Singular += affixes[7];
                }

                //множественное число, первое и второе лица
                _word = rows[indices[0]].Plural;
                if (!_isSoft)
                {
                    rows[indices[0]].Plural += affixes[8];
                    rows[indices[1]].Plural += affixes[8];
                }
                else
                {
                    rows[indices[0]].Plural += affixes[9];
                    rows[indices[1]].Plural += affixes[9];
                }

                //единственное и множественное числа, третье лицо
                _word = rows[indices[2]].Singular;
                if (!_isSoft)
                {
                    rows[indices[2]].Singular += affixes[10];
                    rows[indices[2]].Plural += affixes[10];
                }
                else
                {
                    rows[indices[2]].Singular += affixes[11];
                    rows[indices[2]].Plural += affixes[11];
                }
            }

            // основа + аффикс падежа
            // единственное число
            _word = rows[indices[3]].Singular;
            lastLetter = _word.Last();
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[indices[3]].Singular += affixes[12];
                }
                else
                {
                    rows[indices[3]].Singular += affixes[13];
                }
            }
            else
            {
                if (!_isSoft)
                {
                    rows[indices[3]].Singular += affixes[14];
                }
                else
                {
                    rows[indices[3]].Singular += affixes[15];
                }
            }
            // множественное число
            _word = rows[indices[3]].Plural;
            lastLetter = _word.Last();
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[indices[3]].Plural += affixes[12];
                }
                else
                {
                    rows[indices[3]].Plural += affixes[13];
                }
            }
            else
            {
                if (!_isSoft)
                {
                    rows[indices[3]].Plural += affixes[14];
                }
                else
                {
                    rows[indices[3]].Plural += affixes[15];
                }
            }
        }

        // Төшем килеше
        private void AccusativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = {4, 11, 18, 25};
            string[] affixes = {"ны", "не", "н", "н"};
            GeneralisationForPossessiveAndAccusativeCases(rows, indices, affixes);
        }

        // Чыгыш килеше
        private void AblativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            //единственное число, первое и второе лица
            _word = rows[5].Singular;
            char lastLetter = _word.Last();
            if (_lastException.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[5].Singular += "нан";
                    rows[12].Singular += "нан";
                }
                else
                {
                    rows[5].Singular += "нән";
                    rows[12].Singular += "нән";
                }

                //множественное число, первое и второе лица
                _word = rows[5].Plural;
                if (!_isSoft)
                {
                    rows[5].Plural += "дан";
                    rows[12].Plural += "дан";
                }
                else
                {
                    rows[5].Plural += "дән";
                    rows[12].Plural += "дән";
                }

                //единственное и множественное числа, третье лицо
                _word = rows[19].Singular;
                if (!_isSoft)
                {
                    rows[19].Singular += "ннан";
                    rows[19].Plural += "ннан";
                }
                else
                {
                    rows[19].Singular += "ннән";
                    rows[19].Plural += "ннән";
                }
            }
            else if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[5].Singular += "дан";
                    rows[12].Singular += "дан";
                }
                else
                {
                    rows[5].Singular += "дән";
                    rows[12].Singular += "дән";
                }

                //множественное число, первое и второе лица
                _word = rows[5].Plural;
                if (!_isSoft)
                {
                    rows[5].Plural += "дан";
                    rows[12].Plural += "дан";
                }
                else
                {
                    rows[5].Plural += "дән";
                    rows[12].Plural += "дән";
                }

                //единственное и множественное числа, третье лицо
                _word = rows[19].Singular;
                if (!_isSoft)
                {
                    rows[19].Singular += "дан";
                    rows[19].Plural += "дан";
                }
                else
                {
                    rows[19].Singular += "дән";
                    rows[19].Plural += "дән";
                }
            }
            else
            {
                if (!_isSoft)
                {
                    rows[5].Singular += "тан";
                    rows[12].Singular += "тан";
                }
                else
                {
                    rows[5].Singular += "тән";
                    rows[12].Singular += "тән";
                }

                //множественное число, первое и второе лица
                _word = rows[5].Plural;
                if (!_isSoft)
                {
                    rows[5].Plural += "тан";
                    rows[12].Plural += "тан";
                }
                else
                {
                    rows[5].Plural += "тән";
                    rows[12].Plural += "тән";
                }

                //единственное и множественное числа, третье лицо
                _word = rows[19].Singular;
                if (!_isSoft)
                {
                    rows[19].Singular += "тан";
                    rows[19].Plural += "тан";
                }
                else
                {
                    rows[19].Singular += "тән";
                    rows[19].Plural += "тән";
                }
            }

            // основа + аффикс падежа
            // единственное число
            _word = rows[26].Singular;
            lastLetter = _word.Last();
            if (_lastException.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[26].Singular += "нан";
                }
                else
                {
                    rows[26].Singular += "нән";
                }
            }
            else if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[26].Singular += "дан";
                }
                else
                {
                    rows[26].Singular += "дән";
                }
            }
            else
            {
                if (!_isSoft)
                {
                    rows[26].Singular += "тан";
                }
                else
                {
                    rows[26].Singular += "тән";
                }
            }
            // множественное число
            _word = rows[26].Plural;
            lastLetter = _word.Last();
            if (_lastException.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[26].Plural += "нан";
                }
                else
                {
                    rows[26].Plural += "нән";
                }
            }
            else if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                if (!_isSoft)
                {
                    rows[26].Plural += "дан";
                }
                else
                {
                    rows[26].Plural += "дән";
                }
            }
            else
            {
                if (!_isSoft)
                {
                    rows[26].Plural += "тан";
                }
                else
                {
                    rows[26].Plural += "тән";
                }
            }
        }

        // Урын-вакыт килеше
        private void LocativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = {6, 13, 20, 27};
            string[] affixes =
                {"да", "дә", "да", "дә", "нда", "ндә", "та", "тә", "та", "тә", "та", "тә", "да", "дә", "та", "тә"};
            GeneralisationForDativeAndLocativeCases(rows, indices, affixes);
        }

        private void ArabicAndPersianDeclension()
        {
            switch (_word)
            {
                case "дус":
                {
                    _word += 'т';
                    _isSoft = false;
                    _possessedForms = PossessedForms();
                    List<Row> rows = new List<Row>(28);
                    rows.Add(new Row { Singular = "Первое лицо"});
                    BaseFill(rows, _possessedForms[0], _possessedForms[1]);
                    rows.Add(new Row { Singular = "Второе лицо"});
                    BaseFill(rows, _possessedForms[2], _possessedForms[3]);
                    rows.Add(new Row { Singular = "Третье лицо"});
                    BaseFill(rows, _possessedForms[4], _possessedForms[5]);
                    rows.Add(new Row { Singular = "Основа + аффикс падежа"});
                    _word = Input.Text;
                    BaseFill(rows, _word, "");
                    PossessiveCaseDeclension(rows);
                    DativeCaseDeclension(rows);
                    AccusativeCaseDeclension(rows);
                    AblativeCaseDeclension(rows);
                    LocativeCaseDeclension(rows);
                    Table.ItemsSource = rows;
                    break;
                }
                case "икътисад":
                {
                    _isSoft = false;
                    _possessedForms = PossessedForms();
                    List<Row> rows = new List<Row>(28);
                    rows.Add(new Row { Singular = "Первое лицо"});
                    BaseFill(rows, _possessedForms[0], _possessedForms[1]);
                    rows.Add(new Row { Singular = "Второе лицо"});
                    BaseFill(rows, _possessedForms[2], _possessedForms[3]);
                    rows.Add(new Row { Singular = "Третье лицо"});
                    BaseFill(rows, _possessedForms[4], _possessedForms[5]);
                    rows.Add(new Row { Singular = "Основа + аффикс падежа"});
                    BaseFill(rows, _word, "");
                    PossessiveCaseDeclension(rows);
                    DativeCaseDeclension(rows);
                    AccusativeCaseDeclension(rows);
                    AblativeCaseDeclension(rows);
                    LocativeCaseDeclension(rows);
                    Table.ItemsSource = rows;
                    break;
                }
                case "әдәбият":
                {
                    _isSoft = false;
                    _possessedForms = PossessedForms();
                    List<Row> rows = new List<Row>(28);
                    rows.Add(new Row { Singular = "Первое лицо"});
                    BaseFill(rows, _possessedForms[0], _possessedForms[1]);
                    rows.Add(new Row { Singular = "Второе лицо"});
                    BaseFill(rows, _possessedForms[2], _possessedForms[3]);
                    rows.Add(new Row { Singular = "Третье лицо"});
                    BaseFill(rows, _possessedForms[4], _possessedForms[5]);
                    rows.Add(new Row { Singular = "Основа + аффикс падежа"});
                    BaseFill(rows, _word, "");
                    PossessiveCaseDeclension(rows);
                    DativeCaseDeclension(rows);
                    AccusativeCaseDeclension(rows);
                    AblativeCaseDeclension(rows);
                    LocativeCaseDeclension(rows);
                    Table.ItemsSource = rows;
                    break;
                }
                default:
                    MessageBox.Show("Что делать?");
                    break;
            }
        }

        private void FirstSpecificLetter_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += 'ө';
            InputFocus();
        }

        private void SecondSpecificLetter_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += 'ң';
            InputFocus();
        }

        private void ThirdSpecificLetter_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += 'ү';
            InputFocus();
        }

        private void FourthSpecificLetter_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += 'ә';
            InputFocus();
        }

        private void FifthSpecificLetter_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += 'җ';
            InputFocus();
        }

        private void SixthSpecificLetter_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += 'һ';
            InputFocus();
        }

        private void InputFocus()
        {
            Input.Focus();
            Input.CaretIndex = Input.Text.Length;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = "";
        }

        private void ControlKeys(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Declense_Click(sender, e);
            }

            if (e.Key == Key.Escape)
            {
                Clear_Click(sender, e);
                Input.Focus();
            }
        }
    }

    public class Row
    {
        public string Case { get; set; }

        public string Singular { get; set; }

        public string Plural { get; set; }
    }
}