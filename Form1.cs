using FantasyTennis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FantasyTennisGame
{
    public partial class Form1 : Form
    {
        public Dictionary<int, List<Label>> labelPositioners;
        private System.Windows.Forms.Label addLabelAtLocation(string lblText, Point p)
        {
            System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = p;
            label1.Name = lblText;
            label1.Size = new System.Drawing.Size(35, 13);
            label1.TabIndex = 0;
            label1.Text = lblText;
            // 
            this.Controls.Add(label1);
            this.ResumeLayout(false);

            return label1;
        }

        public Form1(DrawsTree tree, int levels)
        {
            InitializeComponent();

            labelPositioners = new Dictionary<int, List<Label>>();

            DrawTree(tree, levels);
        }

        public void DrawTree(DrawsTree tree, int levels)
        {
            int currentX = 0, currentY = 0;
            int smallJumpY = 15, bigJumpY = 21, nextRoundJump = 150, i = 1;

            for (i = 1; i <= levels; i++)
            {
                List<MatchNode> matches = tree.getNodesAtRound(i);
                List<Label> positionLabels = null;
                int counter = 0;
                if (labelPositioners.ContainsKey(i - 1))
                {
                    positionLabels = labelPositioners[i - 1];
                }

                bool topMatch = true;
                List<Label> listToAddToThePositioners = new List<Label>();

                matches.ForEach((match) =>
                {
                    string name1 = TennisDB.StaticData.IDToName.idToNameMales[match.p1.id];
                    string name2 = TennisDB.StaticData.IDToName.idToNameMales[match.p2.id];
                    Label label1, label2;
                    if (positionLabels == null)
                    {
                        label1 = this.addLabelAtLocation(name1, new Point(currentX, currentY));
                        currentY += smallJumpY;
                        label2 = this.addLabelAtLocation(name2, new Point(currentX, currentY));
                        currentY += bigJumpY;
                    }
                    else
                    {
                        label1 = this.addLabelAtLocation(name1, new Point(currentX, positionLabels.ElementAt(counter).Location.Y));
                        label2 = this.addLabelAtLocation(name2, new Point(currentX, positionLabels.ElementAt(counter+1).Location.Y));
                        counter += 2;
                    }

                    if (topMatch)
                    {
                        listToAddToThePositioners.Add(label2);
                    }
                    else
                    {
                        listToAddToThePositioners.Add(label1);
                    }
                    topMatch = !topMatch;
                });

                this.labelPositioners.Add(i, listToAddToThePositioners);

                currentX += nextRoundJump;
            }

            this.addLabelAtLocation(String.Format("CHAMPION: {0}", TennisDB.StaticData.IDToName.idToNameMales[tree.root.winner.id]), new Point(currentX, (currentY+smallJumpY)/2));

        }
    }
}
