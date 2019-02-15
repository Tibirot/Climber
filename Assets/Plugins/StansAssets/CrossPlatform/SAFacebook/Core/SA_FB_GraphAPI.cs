using System;
using System.Collections.Generic;
using UnityEngine;



namespace SA.Facebook
{
	public class SA_FB_GraphAPI
	{



		/// <summary>
		/// This edge allows you to:
		/// get the User's friends who have installed the app making the query
		/// get the User's total number of friends (including those who have not installed the app making the query)
		/// <para>Requires  <b>"user_friends" </b> permission 
		/// <see cref="https://developers.facebook.com/docs/graph-api/reference/user/friends"/> for information </para>
		/// </summary>
		/// <param name="limit">Result limit </param>
		/// <param name="callback">Request callback </param>
		/// <param name="cursor">Pagination cursor pointer </param>
		public void GetFriends(int limit, Action<SA_FB_GraphFriendsListResult> callback, SA_FB_Cursor cursor = null)
		{


			var request = new SA_FB_RequestBuilder("/me?fields=friends");
			request.AddLimit(limit);
			request.AddCommand("fields", "first_name,id,last_name,name,link,locale,picture");
			request.AddCursor(cursor);

			SA_FB.API(request.RequestString, HttpMethod.GET,
				(IGraphResult graphResult) =>
				{
					var result = new SA_FB_GraphFriendsListResult(graphResult);
					callback.Invoke(result);
				});
		}


		/// <summary>
		/// This edge allows you to:
		/// get the User's friends who have installed the app making the query
		/// get the User's total number of friends (including those who have not installed the app making the query)
		/// <para>This edge is only available to Games (including Gameroom), and requires the <b>"user_friends" </b> permission + Canvas app setup
		/// <see cref="https://developers.facebook.com/docs/graph-api/reference/v2.2/user/invitable_friends"/> for information </para>
		/// </summary>
		/// <param name="limit">Result limit </param>
		/// <param name="callback">Request callback </param>
		/// <param name="cursor">Pagination cursor pointer </param>
		public void GetInvitableFriends(int limit, Action<SA_FB_GraphInvitableFriendsListResult> callback, SA_FB_Cursor cursor = null)
		{

			var request = new SA_FB_RequestBuilder("/me?fields=invitable_friends");
			request.AddLimit(limit);
			request.AddCommand("fields", "first_name,id,last_name,name,link,locale,picture");
			request.AddCursor(cursor);


			SA_FB.API(request.RequestString, HttpMethod.GET,
				(IGraphResult graphResult) =>
				{
					var result = new SA_FB_GraphInvitableFriendsListResult(graphResult);
					callback.Invoke(result);
				});
		}

		internal void GetLoggedInUserInfo(Action<SA_FB_GetUserResult> callback)
		{

			var request = new SA_FB_RequestBuilder("/me?fields=first_name,id,last_name,name,picture");

			SA_FB.API(request.RequestString, HttpMethod.GET,
				(IGraphResult graphResult) =>
				{


					var result = new SA_FB_GetUserResult(graphResult);
					callback.Invoke(result);
				});
		}


		internal void ResolveProfileImageUrl(string id, SA_FB_ProfileImageSize size, Action<string> callback) {
			var request = new SA_FB_RequestBuilder(string.Format("/{0}?fields=picture.type({1})", id, size));
			
			SA_FB.API(request.RequestString, HttpMethod.GET,
				graphResult =>
				{
					var result = new SA_FB_GetProfileImageUrlResult(graphResult);
					callback.Invoke(result.URL);
				});
		}
	}
}
