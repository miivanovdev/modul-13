using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Interaction logic for DialogTransaction.xaml
    /// </summary>
    public partial class DialogTransaction : Window
    {
        public DialogTransaction(TransactionViewModel transactionViewModel)
        {
            InitializeComponent();
            DataContext = transactionViewModel;
        }
    }
}
