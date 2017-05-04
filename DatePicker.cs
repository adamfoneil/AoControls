using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdamOneilSoftware.Controls
{
    // much help from http://www.techpowerup.com/forums/showthread.php?t=70925

    public partial class DatePicker : UserControl, IBoundControl
    {
        private Form _form = null;
        private bool _hasValue = true;
        private DateTime _date = DateTime.Today;
        private string _format = null; 
        private frmMonthSelector popup = null;

        public event EventHandler ValueChanged;

        public DatePicker()
        {
            InitializeComponent();
            InitPopup();
            Value = DateTime.Today;
        }

        private void InitPopup()
        {
            popup = new frmMonthSelector();
            popup.PopupDateSelected += new EventHandler(popup_PopupDateSelected);
        }

        void popup_PopupDateSelected(object sender, EventArgs e)
        {
            Value = popup.Date;
        }

        public string Format
        {
            get { return _format; }
            set
            {
                _format = value;
                UpdateDateDisplay();
            }
        }

        public bool AllowDirectEdit
        {
            get { return tbDate.ReadOnly; }
            set { tbDate.ReadOnly = value; }
        }

        public object Value
        {
            get
            {
                if (_hasValue) return _date.Date;
                return null;
            }
            set
            {
                if (value is DateTime)
                {
                    _date = Convert.ToDateTime(value);
                    tbDate.Text = (!string.IsNullOrEmpty(Format)) ? string.Format("{0:" + Format + "}", value) : value.ToString();
                    _hasValue = true;
                }
                else
                {
                    tbDate.Text = null;
                    _hasValue = false;
                }

                if (ValueChanged != null) ValueChanged(this, new EventArgs());
            }
        }        

        private void tbDate_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbDate.Text))
            {
                DateTime test;
                if (DateTime.TryParse(tbDate.Text, out test))
                {
                    _date = test;
                    _hasValue = true;
                }
                else
                {
                    _hasValue = false;
                    MessageBox.Show("The value you entered was not recognized as a date.");
                    e.Cancel = true;
                }
            }
            else
            {
                _hasValue = false;
            }            
        }        

        private void tbDate_Validated(object sender, EventArgs e)
        {
            UpdateDateDisplay();
        }

        private void UpdateDateDisplay()
        {
            if (_hasValue && !string.IsNullOrEmpty(Format))
            {
                tbDate.Text = string.Format("{0:" + Format + "}", _date);
            }
        }

        public new bool Enabled
        {
            get { return tbDate.Enabled; }
            set
            {
                tbDate.Enabled = value;
                button1.Enabled = value;
            }
        }

        #region IBoundControl Members

        public object BoundValue
        {
            get { return Value; }
            set { Value = value; }
        }

        public void Clear()
        {
            tbDate.Text = null;
            _hasValue = false;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (popup == null || popup.IsDisposed) InitPopup();

            if (!popup.Visible)
            {
                popup.Show(tbDate);
                popup.Location = this.PointToScreen(new Point(tbDate.Location.X, tbDate.Height));
            }
            else
            {
                popup.Hide();
            }            
        }

        void monthView_DateSelected(object sender, DateRangeEventArgs e)
        {
            _date = e.Start;
            _hasValue = true;
        }

        private void tbDate_Enter(object sender, EventArgs e)
        {            
            if (_hasValue) tbDate.Text = _date.ToString("M/d/yy");
        }

        private void DatePicker_Load(object sender, EventArgs e)
        {
            _form = FindForm();
            _form.FormClosing += new FormClosingEventHandler(form_FormClosing);
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (popup != null) popup.Close();
        }
    }

    public class frmMonthSelector : Form
    {
        private MonthCalendar _calendar;
        private DateTime _date;

        public event EventHandler PopupDateSelected;

        public frmMonthSelector()
        {
            FormBorderStyle = FormBorderStyle.None;
            _calendar = new MonthCalendar();
            _calendar.DateSelected += new DateRangeEventHandler(calendar_DateSelected);
            this.Controls.Add(_calendar);
            Size = new Size(227, 162);
        }

        void calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            _date = e.Start;
            this.Hide();
            if (PopupDateSelected != null) PopupDateSelected(this, new EventArgs());
        }

        public DateTime Date
        {
            get { return _date.Date; }
        }
    }
}
