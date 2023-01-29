using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TurtleGraphicsBlazor.Data;
using TurtleGraphicsBlazor.Pages;

namespace TurtleGraphicsBlazor.Services
{
    public class TurtleService
    {

        //command list...
		private List<TurtleCommandList> _command_lists;
		private bool _command_list_needs_update = false;
		private Queue<TurtleCommandList> _command_lists_to_delete = new Queue<TurtleCommandList>();

		public int CommandListCount { get { return _command_lists != null ? _command_lists.Count : 0; } }
        public TurtleCommandList GetCommandList(int idx) => _command_lists[idx];
        public int CommandListPresetCount { get; private set; }

        //application settings.....
		public AppSettings AppSettings { get; private set; }


		

		

        private DatabaseHelper _db;

        public TurtleService(DatabaseHelper db)
        {
            _db = db;
        }
        private IEnumerable<TurtleCommandList> get_preset_command_list()
        {
            TurtleCommandList cl1 = new TurtleCommandList("Circle");
            cl1.Add(TurtleCommandList.CommandTypes.PenColor, 3);

            cl1.Add(TurtleCommandList.CommandTypes.Repeat, 36);
            cl1.Add(TurtleCommandList.CommandTypes.Forward, 10);
            cl1.Add(TurtleCommandList.CommandTypes.Right, 10);
            cl1.Add(TurtleCommandList.CommandTypes.End);
            yield return cl1;

            //two
            TurtleCommandList cl2 = new TurtleCommandList("Star");
            cl2.Add(TurtleCommandList.CommandTypes.PenColor, 7);

            cl2.Add(TurtleCommandList.CommandTypes.Repeat, 5);
            cl2.Add(TurtleCommandList.CommandTypes.Forward, 100);
            cl2.Add(TurtleCommandList.CommandTypes.Right, 144);
            cl2.Add(TurtleCommandList.CommandTypes.End);
            yield return cl2;

            //three
            TurtleCommandList cl3 = new TurtleCommandList("Wheel");

            cl3.Add(TurtleCommandList.CommandTypes.Repeat, 36);
            cl3.Add(TurtleCommandList.CommandTypes.PenColor, 20);

            cl3.Add(TurtleCommandList.CommandTypes.PenUp, 0);
            cl3.Add(TurtleCommandList.CommandTypes.Forward, 65);
            cl3.Add(TurtleCommandList.CommandTypes.PenDown, 20);

            cl3.Add(TurtleCommandList.CommandTypes.Forward, 65);
            cl3.Add(TurtleCommandList.CommandTypes.Backward, 65);

            cl3.Add(TurtleCommandList.CommandTypes.Right, 170);

            cl3.Add(TurtleCommandList.CommandTypes.End);
            yield return cl3;
        }
        public async Task RefreshCommandListAsync()
        {
			_command_lists = new List<TurtleCommandList>();

			_command_lists.AddRange(get_preset_command_list());
            CommandListPresetCount = _command_lists.Count;

            foreach(var c in _db.ReadCommandLists())
            {
                if(c.IsValid())
                {
					_command_lists.Add(c);
                }
            }
        }
        void add_dummy_cmds()
        {
            TurtleCommandList dummy = new TurtleCommandList("dummy");
            dummy.Add(TurtleCommandList.CommandTypes.Forward, 65);
            for(int i = 0; i < 50; i++)
            {
				_command_lists.Add(dummy);
            }

        }
        public async Task UpdateCommandListAsync()
        {
            if (_command_list_needs_update == false && _command_lists_to_delete.Count <= 0)
                return;



            while (_command_lists_to_delete.TryDequeue(out TurtleCommandList tcl))
            {
                _db.DeleteCommandList(tcl.Id);
            }

			//remove preset cmds
			_command_lists.RemoveRange(0, CommandListPresetCount);

            _db.WriteOrUpdateCommandList(_command_lists);

            await RefreshCommandListAsync();
			_command_list_needs_update = false;
        }
        public void DeleteCommandList(int idx)
        {
            _command_lists_to_delete.Enqueue(_command_lists[idx]);
			_command_lists.RemoveAt(idx);
        }
        public void MoveCommandListUp(int idx)
        {
            if (idx > CommandListPresetCount)
            {
                var i = _command_lists.MoveItemUpAt(idx);
                if(i != idx)//swap db id's
                {
                    var temp = _command_lists[i].Id;
					_command_lists[i].Id = _command_lists[idx].Id;
					_command_lists[idx].Id = temp;

					_command_list_needs_update = true;
                }
                
            }
        }
        public void MoveCommandListDown(int idx)
        {
            var i = _command_lists.MoveItemDownAt(idx);
            if (i != idx)//swap db id's
            {
                var temp = _command_lists[i].Id;
				_command_lists[i].Id = _command_lists[idx].Id;
				_command_lists[idx].Id = temp;

				_command_list_needs_update = true;
            }
        }
        public void AddCommandList(TurtleCommandList tcl)
        {
            this._command_list_needs_update = true;
			_command_lists.Add(tcl);
            
        }
        public async Task LoadSettingsFromStorageAsync()
        {
            AppSettings = _db.ReadAppSettings();
            if(AppSettings == null || AppSettings.IsValid() == false)
            {
				AppSettings = new AppSettings()
				{ TurtleImageId = 1, TurtleImageSize = 1, CanvasColor = "DodgerBlue", PenSize = 7, TurtleSpeed = 150, Volume = 50 };

                var w = _db.WriteOrUpdateAppSettings(AppSettings);
            }

		}
        public async Task<bool> UpdateSettingsInStorageAsync()
        {
            bool res = _db.WriteOrUpdateAppSettings(AppSettings);
            return res;

        }

        //public Action<string, string> OnNavigation;
        public bool NavigationBlocked { get; private set; }
        public Action NavigationBlockedCallback { get; private set; }
        public void BlockNavigation(Action handleOnNavBlock)
        {
            this.NavigationBlocked = true;
            this.NavigationBlockedCallback= handleOnNavBlock;
        }
        public void UnblockNavigation()
        {
            this.NavigationBlocked = false;
            this.NavigationBlockedCallback= null;
        }
    }
}
