using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace budgetCSgharp
{
    public partial class Budget : Form
    {
        public Budget()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (BudgetContext db = new BudgetContext())
            {

                db.Database.Delete();
                db.sexes.AddRange(new Sex[] { new Sex(1, "Мужской"), new Sex(2, "Женский") });
                db.SaveChanges();
                IEnumerable<Sex> result = db.sexes.Select(s => s);
                var test = LINQToDataTable(result);
                dataGridView1.DataSource = test;
               
            }
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }
    }
}
