using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SQLite;
using System.Data.SqlClient;
using TurtleGraphicsBlazor.Data;
using LiteDB;

namespace TurtleGraphicsBlazor.Services
{
	public class DatabaseHelper
	{
		private readonly string _db_path = Path.Combine(FileSystem.AppDataDirectory, "turtleappnsq.db");

		private LiteDatabase _con;

        public DatabaseHelper() 
        { 
            this.Connect(); 
        }
        ~DatabaseHelper() 
        {
            this.Disconnect();
        }

        public void Connect()
		{
            if (_con != null) return;

			_con = new LiteDatabase(_db_path);
        }
        public void Disconnect()
        {
            _con?.Dispose();
            _con = null;
        }
        

		public bool WriteOrUpdateAppSettings(AppSettings settings)
		{
            var col = _con.GetCollection(nameof(AppSettings));
			
            var d = new BsonDocument();
			
            d[nameof(settings.TurtleImageId)] = settings.TurtleImageId;
            d[nameof(settings.TurtleImageSize)] = settings.TurtleImageSize;
            d[nameof(settings.PenSize)] = settings.PenSize;
            d[nameof(settings.CanvasColor)] = settings.CanvasColor;
            d[nameof(settings.TurtleSpeed)] = settings.TurtleSpeed;
            d[nameof(settings.Volume)] = settings.Volume;

			
            if (col.FindById(0) != null)
			{
				return col.Update(0, d);
                
            }
			else
			{
                col.Insert(0, d);
				return true;
            }
			return false;
        }

		public AppSettings ReadAppSettings()
		{
			AppSettings settings = null;

            var col = _con.GetCollection(nameof(AppSettings));
            
			try
			{
                var d = col.FindById(0);
                if (d != null)
				{
                    

                    settings = new AppSettings();
                    int _id = d["_id"];
                    settings.TurtleImageId = d[nameof(settings.TurtleImageId)];
                    settings.TurtleImageSize = d[nameof(settings.TurtleImageSize)];
                    settings.PenSize = d[nameof(settings.PenSize)];
                    settings.CanvasColor = d[nameof(settings.CanvasColor)];
                    settings.TurtleSpeed = d[nameof(settings.TurtleSpeed)];
                    settings.Volume = d[nameof(settings.Volume)];
                }
                
            }
			catch (Exception ex) 
			{
				string msg = ex.Message;
			}
            return settings;
        }
        /// <summary>
        /// write or update the command lists
        /// </summary>
        /// <param name="clists">list to write</param>
        /// <returns>total number of lists sucessfully written or updated</returns>
        public int WriteOrUpdateCommandList(List<TurtleCommandList> clists)
        {
            var col = _con.GetCollection(nameof(TurtleCommandList), BsonAutoId.Int32);

            int write_or_updated = 0;
            foreach ( var c in clists)
            {
                if(write_or_update_cmd_list(col, c) > 0)
                {
                    write_or_updated++;
                }
            }
            return write_or_updated;
        }
        public IEnumerable<TurtleCommandList> ReadCommandLists()
        {
            var col = _con.GetCollection(nameof(TurtleCommandList), BsonAutoId.Int32);

            foreach(var d in col.FindAll())
            {
                TurtleCommandList tcl = new TurtleCommandList(d[nameof(tcl.ListName)]);
                tcl.Id = d["_id"];
                foreach(var c in bson_to_commands(d[nameof(tcl.Commands)].AsArray))
                {
                    tcl.Commands.Add(c);
                }

                yield return tcl;
            }
        }
        private IEnumerable<BsonValue> commands_to_bson(List<TurtleCommandList.TurtleCommand> cmds)
        {
            foreach (var cmd in cmds)
            {
                yield return new BsonArray((int)cmd.Command, cmd.Parameter);
            }
        }
        private IEnumerable<TurtleCommandList.TurtleCommand> bson_to_commands(BsonArray arr)
        {
            foreach(var bc in arr)
            {
                var bcarr = bc.AsArray;

                yield return new TurtleCommandList.TurtleCommand((TurtleCommandList.CommandTypes)bcarr[0].AsInt32, bcarr[1]);
            }
        }
        
        public int WriteOrUpdateCommandList(TurtleCommandList clists) =>
            write_or_update_cmd_list(
                _con.GetCollection(nameof(TurtleCommandList), BsonAutoId.Int32), clists);
        private int write_or_update_cmd_list(ILiteCollection<BsonDocument> col, TurtleCommandList clists)
        {
            var d = new BsonDocument();

            d[nameof(clists.ListName)] = clists.ListName;
            d[nameof(clists.Commands)] = new BsonArray(commands_to_bson(clists.Commands));


            var itm = col.FindById(clists.Id);

            if (itm == null)
            {
                return col.Insert(d).AsInt32;
            }
            else
            {
                return col.Update(clists.Id, d) ? clists.Id : -1;
            }
        }
        
        public bool DeleteCommandList(int id)
        {
            var col = _con.GetCollection(nameof(TurtleCommandList), BsonAutoId.Int32);
            return col.Delete(id);
        }
    }
}
