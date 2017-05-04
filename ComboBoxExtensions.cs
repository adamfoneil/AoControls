using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdamOneilSoftware
{
	public static class ComboBoxExtensions
	{
		public static void FillFromEnum<TEnum>(this ComboBox comboBox)
		{
			// help from http://stackoverflow.com/questions/906899/binding-an-enum-to-a-winforms-combo-box-and-then-setting-it

			comboBox.DataSource = GetEnumValueList<TEnum>();
			comboBox.ValueMember = "Key";
			comboBox.DisplayMember = "Value";
		}

		private static object GetEnumValueList<TEnum>()
		{
			return (
				from TEnum enumVal in Enum.GetValues(typeof(TEnum))
				select new KeyValuePair<TEnum, string>(enumVal, enumVal.ToString())).ToList();
		}

		public static void FillFromEnum<TEnum>(this DataGridViewComboBoxColumn comboBoxColumn)
		{
			comboBoxColumn.DataSource = GetEnumValueList<TEnum>();
			comboBoxColumn.ValueMember = "Key";
			comboBoxColumn.DisplayMember = "Value";
		}
	}	
}
