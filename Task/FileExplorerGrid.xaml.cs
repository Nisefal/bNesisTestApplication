using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using bNesis.Sdk.FileStorages.Common;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for ObjectExplorerGrid.xaml
    /// </summary>
    public partial class FileExplorerGrid : UserControl
    {
        const int objectWidth = 80;
        const int objectHeight = 120;
        const int margin = 5;

        private List<Element> elements = new List<Element>();
        private double savedWidth;
        int amountInRow;


        public FileExplorerGrid()
        {
            InitializeComponent();
        }



        public void AddObject(FileExplorerObject element)
        {
            double tempWidth = Field.RenderSize.Width;
            if (savedWidth != tempWidth)
            {
                RepaintGrid();
                savedWidth = tempWidth;                                   // rendered width of Grid we use to display elements
            }

            int amount = elements.Count();                                           // count all elements displayed
            int column = 0;
            if (amount + 1 % amountInRow == 0)
                column = amountInRow;
            else
                column = ((amount + 1) % amountInRow) - 1;                             // calculate number of column of element

            if (column < 0)
                column = 0;

            int row = 0;
            if (amount + 1 % amountInRow == 0)
                row = (int) Math.Truncate((double)((amount + 1) / amountInRow)) - 1;      // calculate number of row of element
            else
                row = (int) Math.Truncate((double)((amount + 1) / amountInRow));      // calculate number of row of element
            
            //element.Margin = new Thickness((objectWidth+margin)*column, (objectHeight+margin)*row, 0, 0);
            elements.Add(new Element(amount+1, row, column, element));

            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            
            Field.Children.Add(element);
        }

        internal void RemoveItem(string fileName)
        {
            Field.Children.Clear();

            Element el = new Element();
            foreach (Element item in elements)
                if (item.feobject.FileName==fileName)
                {
                    el = item;
                    break;
                }

            elements.Remove(el);

            AddChildren(elements);
            //throw new NotImplementedException();
        }

        private void AddObject(Element thisElement)
        {
            FileExplorerObject element = thisElement.feobject;
            double tempWidth = Field.RenderSize.Width;
            if (savedWidth != tempWidth)
            {
                RepaintGrid();
                savedWidth = tempWidth;                                   // rendered width of Grid we use to display elements
            }

            int amount = elements.Count();                                           // count all elements displayed

            int column = ((amount + 1) % amountInRow) - 1;                             // calculate number of column of element
            if (column < 0)
                column = 0;

            int row = (int)Math.Truncate((double)((amount + 1) / amountInRow));      // calculate number of row of element

            //element.Margin = new Thickness((objectWidth + margin) * column, (objectHeight + margin) * row, 0, 0);
            elements.Add(new Element(amount + 1, row, column, element));

            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);

            Field.Children.Add(element);
        }

        public void AddChildren(FileStorageItem[] items)
        {
            RepaintGrid(items.Length);
            foreach (FileStorageItem item in items)
                AddObject(new FileExplorerObject(item));
        }

        private void AddChildren(List<Element> items)
        {
            RepaintGrid(items.Count());
            foreach (Element item in items)
                AddObject(item);
        }

        public void CleanGrid()
        {
            Field.Children.Clear();
        }

        private void RepaintGrid(int amountElementsExpected = 0)
        {
            if (this.IsLoaded)
            {
                savedWidth = Field.RenderSize.Width;

                Field.ColumnDefinitions.Clear();
                Field.RowDefinitions.Clear();

                amountInRow = (int)Math.Truncate(savedWidth / (objectWidth + margin));                                          // count amount of elements possible in row
                for (int i = 0; i <= amountInRow; i++)
                    Field.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((objectWidth + margin)) });     // add columns

                int amount = elements.Count();
                int maxi = 0;
                if (amountElementsExpected!=0)         // just 5 no special mathematic
                    maxi = (int)Math.Truncate((double)(amountElementsExpected / amountInRow));      // calculate number of row of element
                else
                    maxi = (int)Math.Truncate((double)(amount / amountInRow));      // calculate number of row of element


                for (int i = 0; i <= maxi; i++)                                                                                  // add rows
                    Field.RowDefinitions.Add(new RowDefinition() { Height = new GridLength((objectHeight + margin)) });
            }
        }

        struct Element
        {
            public Element(int number, int row, int column, FileExplorerObject feobject)
            {
                this.number = number;
                this.row = row;
                this.column = column;
                this.feobject = feobject;
            }

            public FileExplorerObject feobject;
            public int number;
            public int row;
            public int column;
        }

        private void Field_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Field_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                DropboxConnect.Upload(files[0]);
            }
        }
    }
}
