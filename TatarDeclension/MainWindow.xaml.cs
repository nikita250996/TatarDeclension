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
        private char[] _voicelessedConsonants = { 'п', 'т', 'к', 'ф', 'ч', 'ш', 'щ', 'с', 'х', 'һ', 'ц', 'ъ', 'ь' };
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
        private readonly char[] _softVowels = {'ә', 'ө', 'и', 'ү', 'э'};
        private readonly char[] _hardVowels = {'а', 'о', 'у', 'ы'};
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

                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[1] = _word + "быз";
                }
                else
                {
                    affiliatedForms[1] = _word + "без";
                }

                affiliatedForms[2] = _word + 'ң';

                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[3] = _word + "гыз";
                }
                else
                {
                    affiliatedForms[3] = _word + "гез";
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[4] = affiliatedForms[5] = _word + "сы";
                }
                else
                {
                    affiliatedForms[4] = affiliatedForms[5] = _word + "се";
                }

                return affiliatedForms;
            }

            if (_consonants.Contains(lastLetter))
            {
                //надо дойти до последней гласной
                var buffer = "";
                while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()))
                {
                    buffer += _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                lastLetter = _word.Last();
                buffer.Reverse();
                _word += buffer;

                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[0] = _word + "ым";
                    affiliatedForms[1] = _word + "ыбыз";
                }
                else
                {
                    affiliatedForms[0] = _word + "ем";
                    affiliatedForms[1] = _word + "ебез";
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[2] = _word + "ың";
                    affiliatedForms[3] = _word + "ыгыз";
                }
                else
                {
                    affiliatedForms[2] = _word + "ең";
                    affiliatedForms[3] = _word + "егез";
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    affiliatedForms[4] = affiliatedForms[5] = _word + 'ы';
                }
                else
                {
                    affiliatedForms[4] = affiliatedForms[5] = _word + 'е';
                }

                return affiliatedForms;
            }

            switch (lastLetter)
            {
                case FirstException:
                {
                    //замена п на б
                    _word = _word.Remove(_word.Length - 1);
                    _word += 'б';

                    //надо дойти до последней гласной
                    var buffer = "";
                    while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()))
                    {
                        buffer += _word.Last();
                        _word = _word.Remove(_word.Length - 1);
                    }

                    lastLetter = _word.Last();
                    buffer.Reverse();
                    _word += buffer;

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[0] = _word + "ым";
                        affiliatedForms[1] = _word + "ыбыз";
                    }
                    else
                    {
                        affiliatedForms[0] = _word + "ем";
                        affiliatedForms[1] = _word + "ебез";
                    }

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[2] = _word + "ың";
                        affiliatedForms[3] = _word + "ыгыз";
                    }
                    else
                    {
                        affiliatedForms[2] = _word + "ең";
                        affiliatedForms[3] = _word + "егез";
                    }

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[4] = affiliatedForms[5] = _word + 'ы';
                    }
                    else
                    {
                        affiliatedForms[4] = affiliatedForms[5] = _word + 'е';
                    }

                    return affiliatedForms;
                }
                case SecondException:
                {
                    //замена к на г
                    _word = _word.Remove(_word.Length - 1);
                    _word += 'г';

                    //надо дойти до последней гласной
                    var buffer = "";
                    while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()))
                    {
                        buffer += _word.Last();
                        _word = _word.Remove(_word.Length - 1);
                    }

                    lastLetter = _word.Last();
                    buffer.Reverse();
                    _word += buffer;

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[0] = _word + "ым";
                        affiliatedForms[1] = _word + "ыбыз";
                    }
                    else
                    {
                        affiliatedForms[0] = _word + "ем";
                        affiliatedForms[1] = _word + "ебез";
                    }

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[2] = _word + "ың";
                        affiliatedForms[3] = _word + "ыгыз";
                    }
                    else
                    {
                        affiliatedForms[2] = _word + "ең";
                        affiliatedForms[3] = _word + "егез";
                    }

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[4] = affiliatedForms[5] = _word + 'ы';
                    }
                    else
                    {
                        affiliatedForms[4] = affiliatedForms[5] = _word + 'е';
                    }

                    return affiliatedForms;
                }
                case ThirdException:
                {
                    //замена й на е
                    _word = _word.Remove(_word.Length - 1);
                    _word += 'е';

                    //надо дойти до последней гласной
                    var buffer = "";
                    while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()))
                    {
                        buffer += _word.Last();
                        _word = _word.Remove(_word.Length - 1);
                    }

                    lastLetter = _word.Last();
                    buffer.Reverse();
                    _word += buffer;

                    affiliatedForms[0] = _word + 'м';

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[1] = _word + "быз";
                    }
                    else
                    {
                        affiliatedForms[1] = _word + "без";
                    }

                    affiliatedForms[2] = _word + 'ң';

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[3] = _word + "гыз";
                    }
                    else
                    {
                        affiliatedForms[3] = _word + "гез";
                    }

                    affiliatedForms[4] = affiliatedForms[5] = _word;

                    return affiliatedForms;
                }
                case FourthException:
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

                    return affiliatedForms;
                }
                case FifthException:
                {
                    //удаление ь
                    _word = _word.Remove(_word.Length - 1);

                    affiliatedForms[0] = _word + "ем";
                    affiliatedForms[1] = _word + "ебез";
                    affiliatedForms[2] = _word + "ең";
                    affiliatedForms[3] = _word + "егез";
                    affiliatedForms[4] = affiliatedForms[5] = _word + 'е';

                    return affiliatedForms;
                }
                case SixthException:
                {
                    //надо дойти до последней гласной
                    var buffer = "";
                    while (!_hardVowels.Contains(_word.Last()) && !_softVowels.Contains(_word.Last()))
                    {
                        buffer += _word.Last();
                        _word = _word.Remove(_word.Length - 1);
                    }

                    lastLetter = _word.Last();
                    buffer.Reverse();
                    _word += buffer;

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[0] = _word + "ым";
                        affiliatedForms[1] = _word + "ыбыз";
                    }
                    else
                    {
                        affiliatedForms[0] = _word + "ем";
                        affiliatedForms[1] = _word + "ебез";
                    }

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[2] = _word + "ың";
                        affiliatedForms[3] = _word + "ыгыз";
                    }
                    else
                    {
                        affiliatedForms[2] = _word + "ең";
                        affiliatedForms[3] = _word + "егез";
                    }

                    if (_hardVowels.Contains(lastLetter))
                    {
                        affiliatedForms[4] = affiliatedForms[5] = _word + 'ы';
                    }
                    else
                    {
                        affiliatedForms[4] = affiliatedForms[5] = _word + 'е';
                    }

                    return affiliatedForms;
                }
                case SeventhException:
                {
                    if (_vowels.Contains(_word[_word.Length - 2]))
                    {
                        //замена у на в
                        _word = _word.Remove(_word.Length - 1);
                        _word += 'в';
                    }

                    affiliatedForms[0] = _word + "ым";
                    affiliatedForms[1] = _word + "ыбыз";
                    affiliatedForms[2] = _word + "ың";
                    affiliatedForms[3] = _word + "ыгыз";
                    affiliatedForms[4] = affiliatedForms[5] = _word + 'ы';

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

                    affiliatedForms[0] = _word + "ем";
                    affiliatedForms[1] = _word + "ебез";
                    affiliatedForms[2] = _word + "ең";
                    affiliatedForms[3] = _word + "егез";
                    affiliatedForms[4] = affiliatedForms[5] = _word + 'е';

                    return affiliatedForms;
                }
            }
        }

        private void VowelHarmonyDeclension()
        {
            var rows = new List<Row>(28);
            rows.Add(new Row {Case = "Первое лицо"});
            BaseFill(rows, _affiliatedForms[0], _affiliatedForms[1]);
            rows.Add(new Row { Case = "Второе лицо" });
            BaseFill(rows, _affiliatedForms[2], _affiliatedForms[3]);
            rows.Add(new Row { Case = "Третье лицо" });
            BaseFill(rows, _affiliatedForms[4], _affiliatedForms[5]);
            rows.Add(new Row { Case = "Основа + аффикс падежа" });
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
            rows.Add(new Row { Case = "Баш килеш", Singular = singular, Plural = plural });
            rows.Add(new Row { Case = "Иялек килеше", Singular = singular, Plural = plural });
            rows.Add(new Row { Case = "Юнәлеш килеше", Singular = singular, Plural = plural });
            rows.Add(new Row { Case = "Төшем килеше", Singular = singular, Plural = plural });
            rows.Add(new Row { Case = "Чыгыш килеше", Singular = singular, Plural = plural });
            rows.Add(new Row { Case = "Урын-вакыт килеше", Singular = singular, Plural = plural });
        }

        private void PossessiveCaseDeclension(IReadOnlyList<Row> rows)
        {
            //единственное число, первое и второе лица
            var lastLetter = ' ';
            _word = rows[2].Singular;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[2].Singular += "ның";
                rows[9].Singular += "ның";
            }
            else
            {
                rows[2].Singular += "нең";
                rows[9].Singular += "нең";
            }

            //множественное число, первое и второе лица
            lastLetter = ' ';
            _word = rows[2].Plural;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[2].Plural += "ның";
                rows[9].Plural += "ның";
            }
            else
            {
                rows[2].Plural += "нең";
                rows[9].Plural += "нең";
            }

            //единственное и множественное числа, третье лицо
            lastLetter = ' ';
            _word = rows[16].Singular;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[16].Singular += "ның";
                rows[16].Plural += "ның";
            }
            else
            {
                rows[16].Singular += "нең";
                rows[16].Plural += "нең";
            }

            //основа + аффикс падежа
            lastLetter = ' ';
            _word = Input.Text;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[23].Singular += "ның";
            }
            else
            {
                rows[23].Singular += "нең";
            }
        }

        private void DativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            //единственное число, первое и второе лица
            _word = rows[3].Singular;
            var lastLetter = _word.Last();
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[3].Singular += "а";
                    rows[10].Singular += "а";
                }
                else
                {
                    rows[3].Singular += "ә";
                    rows[10].Singular += "ә";
                }

                //множественное число, первое и второе лица
                lastLetter = ' ';
                _word = rows[3].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[3].Plural += "га";
                    rows[10].Plural += "га";
                }
                else
                {
                    rows[3].Plural += "гә";
                    rows[10].Plural += "гә";
                }

                //единственное и множественное числа, третье лицо
                lastLetter = ' ';
                _word = rows[17].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[17].Singular += "на";
                    rows[17].Plural += "на";
                }
                else
                {
                    rows[17].Singular += "нә";
                    rows[17].Plural += "нә";
                }
            }
            else
            {
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[3].Singular += "ка";
                    rows[10].Singular += "ка";
                }
                else
                {
                    rows[3].Singular += "кә";
                    rows[10].Singular += "кә";
                }

                //множественное число, первое и второе лица
                lastLetter = ' ';
                _word = rows[3].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[3].Plural += "ка";
                    rows[10].Plural += "ка";
                }
                else
                {
                    rows[3].Plural += "кә";
                    rows[10].Plural += "кә";
                }

                //единственное и множественное числа, третье лицо
                lastLetter = ' ';
                _word = rows[17].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[17].Singular += "на";
                    rows[17].Plural += "на";
                }
                else
                {
                    rows[17].Singular += "нә";
                    rows[17].Plural += "нә";
                }
            }

            _word = rows[24].Singular;
            lastLetter = _word.Last();
            //основа + аффикс падежа
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                
                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[24].Singular += "га";
                }
                else
                {
                    rows[24].Singular += "гә";
                }
            }
            else
            {
                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[24].Singular += "ка";
                }
                else
                {
                    rows[24].Singular += "кә";
                }
            }
        }

        private void AccusativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            //единственное число, первое и второе лица
            var lastLetter = ' ';
            _word = rows[4].Singular;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[4].Singular += "ны";
                rows[11].Singular += "ны";
            }
            else
            {
                rows[4].Singular += "не";
                rows[11].Singular += "не";
            }

            //множественное число, первое и второе лица
            lastLetter = ' ';
            _word = rows[4].Plural;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[4].Plural += "ны";
                rows[11].Plural += "ны";
            }
            else
            {
                rows[4].Plural += "не";
                rows[11].Plural += "не";
            }

            //единственное и множественное числа, третье лицо
            lastLetter = ' ';
            _word = rows[18].Singular;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[18].Singular += "н";
                rows[18].Plural += "н";
            }
            else
            {
                rows[18].Singular += "н";
                rows[18].Plural += "н";
            }

            //основа + аффикс падежа
            lastLetter = ' ';
            _word = Input.Text;
            //надо дойти до последней гласной
            while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
            {
                lastLetter = _word.Last();
                _word = _word.Remove(_word.Length - 1);
            }

            if (_hardVowels.Contains(lastLetter))
            {
                rows[25].Singular += "ны";
            }
            else
            {
                rows[25].Singular += "не";
            }
        }

        private void AblativeCaseDeclension(IReadOnlyList<Row> rows)
        {
            //единственное число, первое и второе лица
            _word = rows[5].Singular;
            var lastLetter = _word.Last();
            if (_lastException.Contains(lastLetter))
            {
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = rows[5].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = rows[19].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = rows[5].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = rows[19].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = rows[5].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = rows[19].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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

                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

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
            //единственное число, первое и второе лица
            _word = rows[6].Singular;
            var lastLetter = _word.Last();
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[6].Singular += "да";
                    rows[13].Singular += "да";
                }
                else
                {
                    rows[6].Singular += "дә";
                    rows[13].Singular += "дә";
                }

                //множественное число, первое и второе лица
                lastLetter = ' ';
                _word = rows[6].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[6].Plural += "да";
                    rows[13].Plural += "да";
                }
                else
                {
                    rows[6].Plural += "дә";
                    rows[13].Plural += "дә";
                }

                //единственное и множественное числа, третье лицо
                lastLetter = ' ';
                _word = rows[20].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[20].Singular += "нда";
                    rows[20].Plural += "нда";
                }
                else
                {
                    rows[20].Singular += "ндә";
                    rows[20].Plural += "ндә";
                }
            }
            else
            {
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[6].Singular += "та";
                    rows[13].Singular += "та";
                }
                else
                {
                    rows[6].Singular += "тә";
                    rows[13].Singular += "тә";
                }

                //множественное число, первое и второе лица
                lastLetter = ' ';
                _word = rows[6].Plural;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[6].Plural += "та";
                    rows[13].Plural += "та";
                }
                else
                {
                    rows[6].Plural += "тә";
                    rows[13].Plural += "тә";
                }

                //единственное и множественное числа, третье лицо
                lastLetter = ' ';
                _word = rows[20].Singular;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[20].Singular += "та";
                    rows[20].Plural += "та";
                }
                else
                {
                    rows[20].Singular += "тә";
                    rows[20].Plural += "тә";
                }
            }

            _word = rows[27].Singular;
            lastLetter = _word.Last();
            //основа + аффикс падежа
            if (_sonorantedConsonants.Contains(lastLetter) || _allVowels.Contains(lastLetter))
            {
                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[27].Singular += "да";
                }
                else
                {
                    rows[27].Singular += "дә";
                }
            }
            else
            {
                lastLetter = ' ';
                _word = Input.Text;
                //надо дойти до последней гласной
                while (!_hardVowels.Contains(lastLetter) && !_softVowels.Contains(lastLetter))
                {
                    lastLetter = _word.Last();
                    _word = _word.Remove(_word.Length - 1);
                }

                if (_hardVowels.Contains(lastLetter))
                {
                    rows[27].Singular += "та";
                }
                else
                {
                    rows[27].Singular += "тә";
                }
            }
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