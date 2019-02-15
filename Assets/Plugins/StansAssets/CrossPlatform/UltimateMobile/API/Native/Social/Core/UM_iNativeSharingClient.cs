using System;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.Social
{
    /// <summary>
    /// The ultimate mobile native sharing client.
    /// </summary>
    public interface UM_iNativeSharingClient 
    {

        /// <summary>
        /// Will display the system default sharing dialog.
        /// </summary>
        /// <param name="builder">Dialog builder, that will be used to create UI based on it's properties.</param>
        /// <param name="callback">Operation callback.</param>
        void SystemSharingDialog(UM_ShareDialogBuilder builder, Action<SA_Result> callback);


        /// <summary>
        /// Will display the system facebook sharing dialog.
        /// </summary>
        /// <param name="builder">Dialog builder, that will be used to create UI based on it's properties.</param>
        /// <param name="callback">Operation callback.</param>
        void ShareToFacebook(UM_ShareDialogBuilder builder, Action<SA_Result> callback);


        /// <summary>
        /// Will display the system Instagram sharing dialog.
        /// </summary>
        /// <param name="builder">Dialog builder, that will be used to create UI based on it's properties.</param>
        /// <param name="callback">Operation callback.</param>
        void ShareToInstagram(UM_ShareDialogBuilder builder, Action<SA_Result> callback);


        /// <summary>
        /// Will display the system Twitter sharing dialog.
        /// </summary>
        /// <param name="builder">Dialog builder, that will be used to create UI based on it's properties.</param>
        /// <param name="callback">Operation callback.</param>
        void ShareToTwitter(UM_ShareDialogBuilder builder, Action<SA_Result> callback);


        /// <summary>
        /// Will display the system Whatsap sharing dialog.
        /// </summary>
        /// <param name="builder">Dialog builder, that will be used to create UI based on it's properties.</param>
        /// <param name="callback">Operation callback.</param>
        void ShareToWhatsapp(UM_ShareDialogBuilder builder, Action<SA_Result> callback);


        /// <summary>
        /// Will display the system e-mail sneding dialog.
        /// </summary>
        /// <param name="builder">Dialog builder, that will be used to create UI based on it's properties.</param>
        /// <param name="callback">Operation callback.</param>
        void ShowSendMailDialog(UM_EmailDialogBuilder builder, Action<SA_Result> callback);
    }
}