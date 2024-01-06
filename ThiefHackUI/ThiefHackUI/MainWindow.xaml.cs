using EngineLibrary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThiefHackUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEngineService _engineService;
        private readonly IDatabaseService _databaseService;

        public MainWindow(IEngineService engineService, IDatabaseService databaseService)
        {
            InitializeComponent();
            _engineService = engineService;
            _databaseService = databaseService;

            InitEngineSettings();
            SetParameterByType(EngineHelper.AMMO_PARAMETR, AmmoCheckBox);
        }

        private void SaveOffsets(object sender, RoutedEventArgs e)
        {
            Setting settingMoney = SaveOffset(TextOffsetMoney, EngineHelper.MONEY_PARAMETR);
            Setting settingAmmo = SaveOffset(TextOffsetAmmo, EngineHelper.AMMO_PARAMETR);

            if (settingMoney.Parameter != 0 && settingAmmo.Parameter != 0) 
            {
                _engineService.SetEngineSettings([settingMoney, settingAmmo]);
            }
        }

        private void ResetOffsets(object sender, RoutedEventArgs e)
        {
            TextOffsetAmmo.Text = $"0x{EngineHelper.AMMO_OFFSET:X}";
            TextOffsetMoney.Text = $"0x{EngineHelper.MONEY_OFFSET:X}";

            SaveOffsets(sender, e);
        }

        private void AmmoStateChanged(object sender, RoutedEventArgs e)
        {
            SetParameterByType(EngineHelper.AMMO_PARAMETR, AmmoCheckBox);
        }

        private void MoneyStateChanged(object sender, RoutedEventArgs e) 
        {
            SetParameterByType(EngineHelper.MONEY_PARAMETR, MoneyCheckBox);
        }

        private Setting SaveOffset(TextBox textBox, uint type) 
        {
            Setting result = new();

            try 
            {
                uint offset = Convert.ToUInt32(textBox.Text, 16);
                _databaseService.EditOffsetByType(type, offset);
                textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xAB, 0xAD, 0xB3));
                textBox.Text = $"0x{offset:X}";

                result = new Setting
                {
                    Parameter = type,
                    Offset = offset
                };
            }
            catch 
            {
                textBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0x44, 0x22));
            }

            return result;
        }

        private void InitEngineSettings()
        {
            uint offsetMoney = _databaseService.GetOffsetByType(EngineHelper.MONEY_PARAMETR);
            uint offsetAmmo = _databaseService.GetOffsetByType(EngineHelper.AMMO_PARAMETR);

            if (offsetMoney == 0) { offsetMoney = EngineHelper.MONEY_OFFSET; }
            if (offsetAmmo == 0) { offsetAmmo = EngineHelper.AMMO_OFFSET; }

            _engineService.SetEngineSettings([
                new Setting
                {
                    Parameter = EngineHelper.MONEY_PARAMETR,
                    Offset = offsetMoney
                },
                new Setting
                {
                    Parameter = EngineHelper.AMMO_PARAMETR,
                    Offset = offsetAmmo
                },
            ]);

            TextOffsetAmmo.Text = $"0x{offsetAmmo:X}";
            TextOffsetMoney.Text = $"0x{offsetMoney:X}";
        }

        private void SetParameterByType(uint type, CheckBox checkBox)
        {
            bool value = checkBox.IsChecked ?? false;
            bool result;

            try { result = _engineService.SetParameterByType(type, value); }
            catch { result = false; }

            if (result)
            {
                EllipseIndicator.Fill = System.Windows.Media.Brushes.Green;
            }
            else
            {
                EllipseIndicator.Fill = System.Windows.Media.Brushes.Red;
            }

            checkBox.IsChecked = value && result;
        }
    }
}