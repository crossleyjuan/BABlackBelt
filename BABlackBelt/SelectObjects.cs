using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BABlackBelt
{
    public partial class SelectObjects : Form
    {

        Dictionary<int, Entity> _entities = new Dictionary<int,Entity>();
        List<Entity> _selectedEntities = new List<Entity>();
        private Settings _projectSettings;

        public SelectObjects(string projectFolder)
        {
            InitializeComponent();
            _projectSettings = Settings.getProjectSettings(projectFolder);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAddElements_Click(object sender, EventArgs e)
        {
            foreach (var element in lstObjects.SelectedItems)
            {
                Entity ent = (Entity)element;

                addEntity(ent, new List<int>());
            }
        }

        private void addEntity(Entity ent, List<int> reentrance)
        {
            if (reentrance.Contains(ent.idEnt))
            {
                return;
            }
            _selectedEntities.Add(ent);
            lstSelected.Items.Add(ent);

            reentrance.Add(ent.idEnt);

            if (chkDependant.Checked)
            {
                DataConnection con = DataConnectionFactory.getConnection(_projectSettings["ConnectionString"]);
                DataTable dtAttributes = con.RunQuery("SELECT idEntRelated from attrib where idEnt = " + ent.idEnt + " and idEntRelated is not null");

                foreach (DataRow row in dtAttributes.Rows)
                {
                    int idEntRelated = Convert.ToInt32(row["idEntRelated"]);
                    Entity entRelated = _entities[idEntRelated];

                    addEntity(entRelated, reentrance);
                }
            }
        }

        private void SelectObjects_Load(object sender, EventArgs e)
        {
            DataConnection con = DataConnectionFactory.getConnection(_projectSettings["ConnectionString"]);
            DataTable dtEntities = con.RunQuery("SELECT * from Entity order by EntName");

            foreach (DataRow row in dtEntities.Rows)
            {
                Entity ent = new Entity()
                {
                    idEnt = Convert.ToInt32(row["idEnt"]),
                    entName = Convert.ToString(row["entName"])
                };
                lstObjects.Items.Add(ent);

                _entities.Add(ent.idEnt, ent);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<Entity> SelectedEntities()
        {
            List<Entity> selected = new List<Entity>();

            foreach (var x in lstSelected.SelectedItems)
            {
                selected.Add((Entity)x);
            }
            return selected;
        }
    }
}
