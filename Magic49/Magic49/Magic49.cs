using Smod2.Attributes;
using scp4aiur;
using Smod2;
namespace Magic49
{
	[PluginDetails(
		author = "Albertinchu",
		name = "Magic49",
		description = "El 049 ahora es un cambia formas que quiere matarlos a todos",
		id = "albertinchu.gamemode.Magic49",
		version = "1.0.0",
		SmodMajor = 3,
		SmodMinor = 0,
		SmodRevision = 0
		)]
	public class Magic49 : Plugin
	{

		public override void OnDisable()
		{
			this.Info("Magic49 - Desactivado");
		}

		public override void OnEnable()
		{
			Info("Magic49 - activado.");
		}

		public override void Register()
		{
			GamemodeManager.Manager.RegisterMode(this);
			Timing.Init(this);
			this.AddEventHandlers(new PlayersEvents(this));

		}
		public void RefreshConfig()
		{


		}
	}

}
