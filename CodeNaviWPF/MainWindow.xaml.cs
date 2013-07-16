﻿/*
 * Released as open source by NCC Group Plc - http://www.nccgroup.com/
 * 
 * Developed by Felix Ingram, (felix.ingram@nccgroup.com)
 * 
 * http://www.github.com/nccgroup/tybocer
 * 
 * Released under AGPL. See LICENSE for more information
 */

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using CodeNaviWPF.Models;
using ICSharpCode;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using GraphSharp;
using GraphSharp.Controls;

namespace CodeNaviWPF
{
    public partial class MainWindow : Window
    {
        private GraphProvider gp;
        private bool shownBox = false;
     
        public MainWindow()
        {
            gp = new GraphProvider();

            this.DataContext = gp;
            InitializeComponent();
            Zoomer.ZoomToOriginal();
        }

        private void DirPicker_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gp.UpdateRoot(dialog.SelectedPath);
            }

        }

        private void OnTreeNodeDoubleClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                FileItem fi = item.Header as FileItem;
                if (fi != null)
                {
                    gp.AddFileView(fi);
                }
            }
        }
        private void OnTreeItemExpand(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                DirectoryItem di = item.Header as DirectoryItem;
                if (di != null)
                {
                    gp.ExpandDirectory(di);
                }
            }
        }

        private void SearchString(object sender, RoutedEventArgs e)
        {
            TextArea textarea = e.OriginalSource as TextArea;
            PocVertex v = (PocVertex)((VertexControl)e.Source).Vertex;
            if (textarea != null)
            {
                string selected_text = textarea.Selection.GetText();
                if (selected_text != null && selected_text != "")
                {
                    gp.PerformSearch(selected_text, v);
                }
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SearchResult result = (SearchResult)((System.Windows.Controls.DataGridRow)sender).Item;
            gp.AddFileView(new FileItem { FileName = result.FileName, FullPath = result.FullPath, Extension = result.Extension, RelPath = result.RelPath });
            if (!shownBox) System.Windows.MessageBox.Show("Obviously that new box should be attached to the search results... TODO");
            shownBox = true;
        }
    }

    public static class Commands
    {
        public static readonly RoutedUICommand SearchString = new RoutedUICommand("Search String", "SearchString", typeof(MainWindow));
    }
}
