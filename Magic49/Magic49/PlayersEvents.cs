using System;
using System.Collections.Generic;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2;
using scp4aiur;
using Smod2.EventSystem.Events;
using System.Linq;
namespace Magic49
{
	partial class PlayersEvents : IEventHandlerPlayerDie, IEventHandlerSetRole, IEventHandlerCheckRoundEnd, IEventHandlerRoundStart, 
		IEventHandlerSetSCPConfig, IEventHandlerPlayerDropItem, IEventHandlerElevatorUse, IEventHandlerSetConfig, IEventHandlerWaitingForPlayers
	{
	   
		private Magic49 plugin;
		public PlayersEvents(Magic49 plugin)
		{
			this.plugin = plugin;
		}
		static Dictionary<int, bool> Cambiaform = new Dictionary<int, bool>();
		//////////////////////////////////////////////////Variables////////////////////////////////////////////

		int camfor = 0;
		static int jugadores = PluginManager.Manager.Server.NumPlayers;
		int cntdrin = 0;		
		int salud;			
		string scientist;
		int jugadore = 0;
		bool scientistdead = false;
		int murders;
		int cont = 0;
		/////////////////////////////////////////////////Mini juego//////////////////////////////////////////////////


		public static IEnumerable<float> Muerte(Player player)
		{
			yield return 900f;
			player.ChangeRole(Role.SCP_173);
			player.Kill(DamageType.DECONT);

		}

		public static IEnumerable<float> MVP2(Player player,Vector position)
		{
			int contador = 0;
			Vector playerpos;
			while (contador == 0)
			{
				playerpos = player.GetPosition();
				yield return 5f;
				if ((player.GetPosition() == playerpos)&&(player.GetCurrentItemIndex() == 17))
				{
					player.Teleport(position);
					contador = 1;

				}
				yield return 5f;
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			jugadore = jugadore - 1;
			if(ev.Player.SteamId == scientist) { scientistdead = true; }
			if ((jugadore <= 5) && (PluginManager.Manager.Server.NumPlayers > 15)) {
				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if (player.SteamId == scientist)
					{
						player.SetGodmode(false);
					}
				}
			}    
			if(Cambiaform.ContainsKey(ev.Player.PlayerId)){ murders = murders + 1; }
			if(murders == 2)
			{

				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if (player.TeamRole.Role != Role.SCIENTIST) { player.ChangeRole(Role.SCIENTIST);}
				}
			}
			if ((jugadore <= 3)&&(PluginManager.Manager.Server.NumPlayers < 15)){
				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if (player.SteamId == scientist)
					{
						player.SetGodmode(false);
					}
				}

			}
			if (Cambiaform.ContainsKey(ev.Killer.PlayerId))
			{
				ev.Killer.GiveItem(ItemType.RADIO);
				ev.Killer.SetAmmo(AmmoType.DROPPED_9, 500);
				ev.Killer.GiveItem(ItemType.USP);
				salud = ev.Killer.GetHealth();
				ev.Killer.ChangeRole((ev.Player.TeamRole.Role), false, false);
				ev.Killer.SetHealth(salud);
				if (!ev.Killer.HasItem(ItemType.COIN)){ ev.Killer.GiveItem(ItemType.COIN); }
			    ev.Killer.AddHealth(10); 
			}
			
			
		}
		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			Random s = new Random();
			
			int ale = s.Next(0, 100);
		   
			
			
				if(ev.Player.TeamRole.Role == Role.SCP_049)
				{
				ev.Player.SetHealth(300);
				Cambiaform.Add(ev.Player.PlayerId, false);
				ev.Player.Teleport(PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_173)[1]);
				Timing.Run(Muerte(ev.Player));
				}

			if ((ev.Player.TeamRole.Role == Role.SCIENTIST)&&(cont == 0))
			{
					scientist = ev.Player.SteamId;
					ev.Player.GiveItem(ItemType.P90);
					ev.Player.SetAmmo(AmmoType.DROPPED_5,300);
					ev.Player.SetAmmo(AmmoType.DROPPED_7, 300);
					ev.Player.SetAmmo(AmmoType.DROPPED_9, 300);
					ev.Player.SetGodmode(true);
					cont = 1;
		    }
			if((ev.Player.TeamRole.Role ==Role.SCIENTIST)&&(cont == 1)&&(!Cambiaform.ContainsKey(ev.Player.PlayerId)))
			{
					ev.Player.ChangeRole(Role.CLASSD, false, false);                    
			}
			  
			if((ev.Player.TeamRole.Role == Role.CLASSD)&&(!Cambiaform.ContainsKey(ev.Player.PlayerId)))
			{
					ev.Player.ChangeRole(Role.CHAOS_INSURGENCY, false, false);
			}
			
		}
		public void OnSetSCPConfig(SetSCPConfigEvent ev)
		{
			ev.Ban079 = true;
			ev.Ban096 = true;
			ev.Ban106 = true;
			ev.Ban173 = true;
			ev.Ban939_53 = true;
			ev.Ban939_89 = true;
			
			
			ev.SCP049amount = 2;
			
		}

		public void OnPlayerDropItem(PlayerDropItemEvent ev)
		{
			if((Cambiaform.ContainsKey(ev.Player.PlayerId))&&(ev.Item.ItemType == ItemType.COIN))
			{
			   
			   ev.ChangeTo = ItemType.NULL;
			   ev.Player.GiveItem(ItemType.COIN);
			   Timing.Run(MVP2(ev.Player,ev.Player.GetPosition()));
			}
		
		}

		public void OnCheckRoundEnd(CheckRoundEndEvent ev)
		{
			if((ev.Server.NumPlayers < 4)&&(scientistdead == true)) { ev.Status = ROUND_END_STATUS.SCP_VICTORY; }
			else { ev.Status = ROUND_END_STATUS.MTF_VICTORY; }
			
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			PluginManager.Manager.Server.Map.GetDoors();
			jugadore = PluginManager.Manager.Server.NumPlayers;
		}

	 

		public void OnElevatorUse(PlayerElevatorUseEvent ev)
		{
			ev.AllowUse = false;
		}

		public void OnSetConfig(SetConfigEvent ev)
		{
			switch (ev.Key)
			{
				case "minimum_MTF_time_to_spawn":
					ev.Value = 1100;
					break;
				case "maximum_MTF_time_to_spawn":
					ev.Value = 1200;
					break;

			}
		}

		
		

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			cntdrin = 0;
			murders = 0;
			salud = 0;
			jugadore = 0;
			scientist = "0";
			scientistdead = false;
			cont = 0;
		}
	}

}   
