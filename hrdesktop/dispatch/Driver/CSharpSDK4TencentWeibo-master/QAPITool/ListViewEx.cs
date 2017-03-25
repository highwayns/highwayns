using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QAPITool
{
    public class ListViewEx : System.Windows.Forms.ListView
    {

        private ListViewItem.ListViewSubItem m_currentLVSubItem;
        private System.Windows.Forms.TextBox editBox;
        private Font m_fontEdit;
        private Color m_bgcolorEdit;

        public ListViewEx()
        {
            editBox = new System.Windows.Forms.TextBox();
            this.EditFont = this.Font;

            this.EditBgColor = Color.LightBlue;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SMKMouseDown);
            this.GridLines = true;

            editBox.Size = new System.Drawing.Size(0, 0);
            editBox.Location = new System.Drawing.Point(0, 0);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.editBox });
            editBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
            editBox.LostFocus += new System.EventHandler(this.FocusOver);
            editBox.AutoSize = true;
            editBox.Font = this.EditFont;
            editBox.BackColor = this.EditBgColor;
            editBox.BorderStyle = BorderStyle.FixedSingle;
            editBox.Hide();
            editBox.Text = "";
        }

        public Font EditFont
        {
            get { return this.m_fontEdit; }
            set
            {
                this.m_fontEdit = value;
                this.editBox.Font = this.m_fontEdit;
            }
        }

        public Color EditBgColor
        {
            get { return this.m_bgcolorEdit; }
            set
            {
                this.m_bgcolorEdit = value;
                this.editBox.BackColor = this.m_bgcolorEdit;
            }
        }

        private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                m_currentLVSubItem.Text = editBox.Text;
                editBox.Hide();
            }

            if (e.KeyChar == 27)
                editBox.Hide();
        }

        private void FocusOver(object sender, System.EventArgs e)
        {
            m_currentLVSubItem.Text = editBox.Text;
            editBox.Hide();
            m_currentLVSubItem = null;
            editBox.Text = null;
        }

        public void SMKMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_currentLVSubItem = null;
            Point tmpPoint = new Point(e.X, e.Y);
            m_currentLVSubItem = this.HitTest(tmpPoint).SubItem;
            if (m_currentLVSubItem == null)
            {
                return;
            }

            editBox.Size = new System.Drawing.Size(m_currentLVSubItem.Bounds.Width, m_currentLVSubItem.Bounds.Height);
            editBox.Location = new System.Drawing.Point(m_currentLVSubItem.Bounds.X, m_currentLVSubItem.Bounds.Y);
            editBox.Show();
            editBox.Text = m_currentLVSubItem.Text;
            editBox.SelectAll();
            editBox.Focus();
        }

        public void DelSelItem()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Checked == true)
                {
                    this.Items.RemoveAt(i);
                    i--;
                }
            }
        }
		
		public void DeleteAllItem()
		{
			while(0< this.Items.Count)
				this.Items.RemoveAt(0);
		}

        public void AddItem(string[] param)
        {
            ListViewItem liv = new ListViewItem("");
            for (int i = 0; i < param.Length; i++)
            {
                liv.SubItems.Add(param[i]);
            }

            this.Items.Add(liv);
        }

        public void GetColumnItem(int index, List<string> result)
        {
            if (index == 0)
                throw new Exception("不能取第0 column的item值");

            for (int i = 0; i < this.Items.Count; i++)
            {
                ListViewItem liv;
                liv = this.Items[i];
                result.Add(liv.SubItems[index].Text);
            }

        }
    }


}
