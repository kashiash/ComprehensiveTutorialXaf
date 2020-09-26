using ComprehensiveTutorialXaf.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.Win.Controllers
{
  public  class WinListViewController: ViewController<ListView>
    {
        GridListEditor gridListEditor = null;
        public WinListViewController()
        {
            ViewControlsCreated += WinAlternatingRowsController_ViewControlsCreated;
        }



        private void WinAlternatingRowsController_ViewControlsCreated(object sender, EventArgs e)
        {
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor != null)
            {
                GridView gridView = listEditor.GridView;
                gridView.OptionsView.EnableAppearanceOddRow = true;
                //  gridView.Appearance.OddRow.BackColor = Color.FromArgb(244, 244, 244);
                gridView.OptionsView.ShowFooter = false;
                // gridView.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
                gridView.OptionsPrint.ExpandAllGroups = false;
                //   właczamy filtry pod nagłowkami
                gridView.OptionsView.ShowAutoFilterRow = true;
                // właczamy scroll - ustaw false

                //checboxy do zaznaczania
                //gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;


                Object currentObject = this.View.Model.ModelClass;
                if (currentObject != null && currentObject.GetType() == typeof(DemoTask))
                {
                    // aby otrzymać multiline na gridzie
                    // właczamy zmiane rozmiru kolumn
                    gridView.OptionsView.RowAutoHeight = true;
                    gridView.OptionsView.ColumnAutoWidth = true;
                }

                else
                {
                    gridView.OptionsView.ColumnAutoWidth = false;
                }

            }
        }


    }
}
