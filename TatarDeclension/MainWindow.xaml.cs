using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        private readonly char[] _softVowels = {'ә', 'ө', 'и', 'ү', 'э', 'е', 'я'};
        private readonly char[] _hardVowels = {'а', 'о', 'у', 'ы', 'ю'};
        private string _word;
        private string[] _affiliatedForms;
        private readonly char[] _lastException = {'м', 'н', 'ң'};

        private readonly char[] _sonorantedConsonants =
            {'б', 'в', 'г', 'д', 'ж', 'җ', 'з', 'л', 'м', 'н', 'ң', 'р', 'й'};

        private readonly char[] _allVowels = {'ә', 'ө', 'и', 'ү', 'э', 'а', 'о', 'у', 'ы', 'ю'};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Declense_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = Input.Text.ToLower();
            _word = Input.Text;
            if (_word == "")
            {
                MessageBox.Show("Пожалуйста, введите слово, которое вы хотите просклонять");
                return;
            }

            //слово подчиняется закону сингармонизма
            if (IsInVowelHarmony.IsChecked == true)
            {
                //добавление аффиксов принадлежности
                _affiliatedForms = AffiliatedForms();
                _lastLetter = _word.Last();
                //добавление аффиксов падежей
                VowelHarmonyDeclension();
                return;
            }

            //слово не подчиняется закону сингармонизма
            ArabicAndPersianDeclension();
        }

        private string[] AffiliatedForms()
        {
            var affiliatedForms = new string[6];
            var lastLetter = _word.Last();
            if (_vowels.Contains(lastLetter))
            {
                affiliatedForms[0] = _word + 'м';
                affiliatedForms[2] = _word + 'ң';
                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[1] = _word + "быз";
                    affiliatedForms[3] = _word + "гыз";
                    affiliatedForms[4] = affiliatedForms[5] = _word + "сы";
                }
                else
                {
                    affiliatedForms[1] = _word + "без";
                    affiliatedForms[3] = _word + "гез";
                    affiliatedForms[4] = affiliatedForms[5] = _word + "се";
                }

                return affiliatedForms;
            }

            if (_consonants.Contains(lastLetter))
            {
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
                {
                    AffiliateHard(affiliatedForms);
                }
                else
                {
                    AffiliateSoft(affiliatedForms);
                }

                return affiliatedForms;
            }

            switch (lastLetter)
            {
                case FirstException:
                {
                    //замена п на б
                    return AffiliateException('б', affiliatedForms);
                }
                case SecondException:
                {
                    //замена к на г
                    return AffiliateException('г', affiliatedForms);
                }
                case ThirdException:
                {
                    //замена й на е
                    _word = _word.Remove(_word.Length - 1);
                    _word += 'е';
                    lastLetter = GetLastVowel();
                    affiliatedForms[0] = _word + 'м';
                    affiliatedForms[2] = _word + 'ң';
                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[1] = _word + "быз";
                        affiliatedForms[3] = _word + "гыз";
                    }
                    else
                    {
                        affiliatedForms[1] = _word + "без";
                        affiliatedForms[3] = _word + "гез";

                    }

                    affiliatedForms[4] = affiliatedForms[5] = _word;
                    return affiliatedForms;
                }
                case FourthException:
                {
                    AffiliateSoft(affiliatedForms);
                    return affiliatedForms;
                }
                case FifthException:
                {
                    //удаление ь
                    _word = _word.Remove(_word.Length - 1);
                    AffiliateSoft(affiliatedForms);
                    return affiliatedForms;
                }
                case SixthException:
                {
                    return AffiliateException(' ', affiliatedForms);
                }
                case SeventhException:
                {
                    if (_vowels.Contains(_word[_word.Length - 2]))
                    {
                        //замена у на в
                        _word = _word.Remove(_word.Length - 1);
                        _word += 'в';
                    }

                    AffiliateHard(affiliatedForms);
                    return affiliatedForms;
                }
                default:
                {
                    if (_vowels.Contains(_word[_word.Length - 2]))
                    {
                        //замена ү на в
                        _word = _word.Remove(_word.Length - 1);
                        _word += 'в';
                    }

                    AffiliateSoft(affiliatedForms);
                    return affiliatedForms;
                }
            }
        }

        private char GetLastVowel()
        {
            var buffer = "";
            while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()))
            {
                buffer += _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            var lastVowel = _word.Last();
            _word += Reverse(buffer);
            return lastVowel;
        }

        private string[] AffiliateException(char consonantedLetter, string[] affiliatedForms)
        {
            if (consonantedLetter != ' ')
            {
                _word = _word.Remove(_word.Length - 1);
                _word += consonantedLetter;
            }

            var lastLetter = GetLastVowel();
            if (_hardVowels.Contains(lastLetter))
            {
                AffiliateHard(affiliatedForms);
            }
            else
            {
                AffiliateSoft(affiliatedForms);
            }

            return affiliatedForms;
        }

        private void AffiliateHard(IList<string> affiliatedForms)
        {
            affiliatedForms[0] = _word + "ым";
            affiliatedForms[1] = _word + "ыбыз";
            affiliatedForms[2] = _word + "ың";
            affiliatedForms[3] = _word + "ыгыз";
            affiliatedForms[4] = affiliatedForms[5] = _word + 'ы';
        }

        private void AffiliateSoft(IList<string> affiliatedForms)
        {
            affiliatedForms[0] = _word + "ем";
            affiliatedForms[1] = _word + "ебез";
            affiliatedForms[2] = _word + "ең";
            affiliatedForms[3] = _word + "егез";
            if (_people.Contains(_word))
            {
                affiliatedForms[4] = affiliatedForms[5] = _word + "се";
            }
            else
            {
                affiliatedForms[4] = affiliatedForms[5] = _word + 'е';
            }
        }

        public string Reverse(string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void VowelHarmonyDeclension()
        {
            var rows = new List<Row>(28);
            rows.Add(new Row {Case = "Первое лицо"});
            BaseFill(rows, _affiliatedForms[0], _affiliatedForms[1]);
            rows.Add(new Row {Case = "Второе лицо"});
            BaseFill(rows, _affiliatedForms[2], _affiliatedForms[3]);
            rows.Add(new Row {Case = "Третье лицо"});
            BaseFill(rows, _affiliatedForms[4], _affiliatedForms[5]);
            rows.Add(new Row {Case = "Основа + аффикс падежа"});
            _word = Input.Text;
            BaseFill(rows, _word, "");
            PossessiveCaseDeclension(rows);
            DativeCaseDeclension(rows);
            AccusativeCaseDeclension(rows);
            AblativeCaseDeclension(rows);
            LocativeCaseDeclension(rows);
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

        private void PossessiveCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = {2, 9, 16, 23};
            string[] affixes = {"ның", "нең", "ның", "нең"};
            GeneralisationForPossessiveAndAccusativeCases(rows, indices, affixes);
        }

        private void GeneralisationForPossessiveAndAccusativeCases(IReadOnlyList<Row> rows, IReadOnlyList<int> indices, IReadOnlyList<string> affixes)
        {
            //единственное число, первое и второе лица
            _word = rows[indices[0]].Singular;
            var lastLetter = GetLastVowel();
            if (_hardVowels.Contains(lastLetter))
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
            lastLetter = GetLastVowel();
            if (_hardVowels.Contains(lastLetter))
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
            lastLetter = GetLastVowel();
            if (_hardVowels.Contains(lastLetter))
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
            lastLetter = GetLastVowel();

            if (_hardVowels.Contains(lastLetter))
            {
                rows[indices[3]].Singular += affixes[0];
            }
            else
            {
                rows[indices[3]].Singular += affixes[1];
            }
        }

        private void DativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = {3, 10, 17, 24};
            string[] affixes = { "а" , "ә", "га", "гә", "на", "нә", "ка", "кә", "ка", "кә", "на", "нә", "га", "гә", "ка", "кә" };
            GeneralisationForDativeAndLocativeCases(rows, indices, affixes);
        }

        private void GeneralisationForDativeAndLocativeCases(IReadOnlyList<Row> rows, IReadOnlyList<int> indices, IReadOnlyList<string> affixes)
        {
            //единственное число, первое и второе лица
            _word = rows[indices[0]].Singular;
            var lastLetter = _word.Last();
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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

            _word = rows[indices[3]].Singular;
            lastLetter = _word.Last();
            //основа + аффикс падежа
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                _word = Input.Text;
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                _word = Input.Text;
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
                {
                    rows[indices[3]].Singular += affixes[14];
                }
                else
                {
                    rows[indices[3]].Singular += affixes[15];
                }
            }
        }
        
        private void AccusativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = { 4, 11, 18, 25 };
            string[] affixes = { "ны", "не", "н", "н" };
            GeneralisationForPossessiveAndAccusativeCases(rows, indices, affixes);
        }

        private void AblativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            //единственное число, первое и второе лица
            _word = rows[5].Singular;
            var lastLetter = _word.Last();
            if (_lastException.Contains(lastLetter))
            {
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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

            _word = rows[26].Singular;
            lastLetter = _word.Last();
            //основа + аффикс падежа
            if (_lastException.Contains(lastLetter))
            {
                _word = Input.Text;
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                _word = Input.Text;
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
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
                _word = Input.Text;
                lastLetter = GetLastVowel();
                if (_hardVowels.Contains(lastLetter))
                {
                    rows[26].Singular += "тан";
                }
                else
                {
                    rows[26].Singular += "тән";
                }
            }
        }

        private void LocativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            int[] indices = { 6, 13, 20, 27 };
            string[] affixes = { "да", "дә", "да", "дә", "нда", "ндә", "та", "тә", "та", "тә", "та", "тә", "да", "дә", "та", "тә" };
            GeneralisationForDativeAndLocativeCases(rows, indices, affixes);
        }

        private void ArabicAndPersianDeclension()
        {
            MessageBox.Show("Что делать?");
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
    }

    public class Row
    {
        public string Case { get; set; }

        public string Singular { get; set; }

        public string Plural { get; set; }
    }
}