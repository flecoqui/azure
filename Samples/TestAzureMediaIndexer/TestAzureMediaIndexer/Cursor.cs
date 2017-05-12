﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAzureMediaIndexer
{
    public class CursorHandler
        : IDisposable
    {
        public CursorHandler(Cursor cursor = null)
        {
            _saved = Cursor.Current;
            Cursor.Current = cursor ?? Cursors.WaitCursor;
        }

        public void Dispose()
        {
            if (_saved != null)
            {
                Cursor.Current = _saved;
                _saved = null;
            }
        }

        private Cursor _saved;
    }

}
