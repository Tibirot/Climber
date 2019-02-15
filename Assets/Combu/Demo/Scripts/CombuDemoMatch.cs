using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Combu;

/*
 * This script include sample code to demonstrate how to work with Matches and Tournaments
 * by using Unity UI components.
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class CombuDemoMatch : CombuDemoScene
{
	public Text textTournament;
	public InputField tournamentId;
	public Text errorTournament;

	public Text textMatch;
	public InputField matchId;
	public InputField matchScore;
	public Text errorMatch;

	
	string LogMatch (Match match)
	{
		string log = "Match ID: " + match.id;
		int n_round = 0;
		foreach (Match.MatchRoundData round in match.rounds)
		{
			n_round++;
			log += "\n- Round " + n_round + " > ";
			for (int i = 0; i < round.users.Count; ++i)
			{
				log += (i > 0 ? "-" : "") + round.users[i].user.userName;
			}
			log += " = ";
			for (int i = 0; i < round.scores.Count; ++i)
			{
				string score = "[NA]";
				if (round.scores[i].dateScore.HasValue && round.scores[i].dateScore != null)
					score = round.scores[i].score.ToString();
				log += (i > 0 ? "-" : "") + score;
			}
		}
		return log;
	}

	public void TournamentsList ()
	{
		textTournament.text = "Loading...";
		errorTournament.text = "";
		Tournament.Load(false, null, (Tournament[] tournaments) => {

			if (tournaments.Length == 0)
			{
				textTournament.text = "No active Tournaments";
			}
			else
			{
				textTournament.text = "";
				foreach (Tournament t in tournaments)
				{
					if (!string.IsNullOrEmpty(textTournament.text))
						textTournament.text += "\n";
					textTournament.text += "Tournament ID: " + t.id + " - Matches: " + t.matches.Count + " (" + (t.finished ? "FINISHED" : "IN PROGRESS") + ")";
				}
			}
			
		});
	}
	
	public void TournamentMatches ()
	{
		long idTournament = 0;
		long.TryParse(tournamentId.text, out idTournament);

		if (idTournament > 0)
		{
			textTournament.text = "Loading...";
			errorTournament.text = "";
			Tournament.Load(idTournament, (Tournament tournament) => {
				if (tournament == null)
				{
					errorTournament.text = "Invalid Tournament";
				}
				else
				{
					textTournament.text = "";
					foreach (Match match in tournament.matches)
					{
						textTournament.text += (string.IsNullOrEmpty(textTournament.text) ? "" : "\n") + LogMatch(match);
					}
				}
			});
		}
		else
		{
			TournamentsList();
		}
	}
	
	public void QuickTournament ()
	{
		string oldText = textTournament.text;
		textTournament.text = "Loading...";
		errorTournament.text = "";
		// First page of 2 results
		User.Random( null, 4, (User[] users) => {
			
			Tournament t = Tournament.QuickTournament(users);

			if (t.matches.Count == 0)
			{
				errorTournament.text = "No matches";
				textTournament.text = oldText;
			}
			else
			{
				t.title = "My Tournament 1";
				t.customData["Key1"] = "Value";
				
				t.Save((bool success, string error) => {
					
					textTournament.text = "Tournament saved: " + success + " - matches: " + t.matches.Count;
					
				});
			}
			
		});
	}
	
	public void MatchList ()
	{
		textMatch.text = "Loading...";
		Match.Load(0, true, string.Empty, (Match[] matches) => {
			if (matches.Length == 0)
			{
				textTournament.text = "No active Matches";
			}
			else
			{
				textMatch.text = "";
				foreach (Match match in matches)
				{
					textMatch.text += (string.IsNullOrEmpty(textMatch.text) ? "" : "\n") + LogMatch(match);
				}
			}
		});
	}

	public void MatchLoad ()
	{
		errorMatch.text = "";
		long idMatch = 0;
		long.TryParse(matchId.text, out idMatch);
		if (idMatch > 0)
		{
			textMatch.text = "Loading...";
			Match.Load(idMatch, (Match match) => {
				if (match == null)
					errorMatch.text = "Invalid Match";
				else
					textMatch.text = LogMatch(match);
			});
		}
		else
		{
			MatchList();
		}
	}
	
	public void MatchScore ()
	{
		long idMatch = 0;
		float score = 0;
		long.TryParse(matchId.text, out idMatch);
		float.TryParse(matchScore.text, out score);
		if (idMatch > 0)
		{
			textMatch.text = "Loading...";
			Match.Load(idMatch, (Match match) => {
				
				match.Score(score, (bool success, string error) => {
					errorMatch.text = error;
					textMatch.text = string.Format("Score: {0}\n{1}", success, LogMatch(match));
				});
				
			});
		}
		else
		{
			errorMatch.text = "Invalid Match";
		}
	}
	
	public void QuickMatch ()
	{
		textMatch.text = "Loading...";
		Match.QuickMatch(false, null, 1, (Match match) => {
			
			if (match == null)
				textMatch.text = "Create Match failed";
			else
				textMatch.text = LogMatch(match);

		});
	}
}
