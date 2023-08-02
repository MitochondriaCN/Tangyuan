using Tangyuan.Data;
namespace Tangyuan.Pages;

public partial class FollowingAndFansPage : ContentPage,IQueryAttributable
{
    UserInfo ui;

    /// <summary>
    /// CollectionView绑定对象。
    /// </summary>
    public class UserListItem
    {
        public uint UserID { get; set; }
        public ImageSource Avatar { get; set; }
        public string UserName { get; set; }
        public bool IsMenuEnabled { get; set; }

    }

	public FollowingAndFansPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count != 0)
        {
            UICompleter(uint.Parse(query["id"].ToString()), query["type"].ToString() == "following" ? true : false);
            query.Clear();
        }
    }

    private async Task UICompleter(uint id, bool isFollowing)
    {
        adiLoading.IsRunning = true;

        ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(id));
        Title = ui.Nickname + (isFollowing ? " 正在关注" : " 的粉丝");
        uint[] itemsid = isFollowing ? await Task.Run(() => SQLDataHelper.GetFollowingsByUserID(id)) :
            await Task.Run(() => SQLDataHelper.GetFansByUserID(id));
        List<UserListItem> listitems = new List<UserListItem>();
        foreach (var v in itemsid)
        {
            UserInfo uinfo = await Task.Run(() => SQLDataHelper.GetUserInfoByID(v));
            listitems.Add(new()
            {
                UserID = uinfo.UserID,
                UserName = uinfo.Nickname,
                Avatar = ImageSource.FromUri(new Uri(uinfo.Avatar)),
                IsMenuEnabled = id == LoginStatusManager.LoggedInUserID && isFollowing ? true : false
            });
        }
        clvUserList.ItemsSource = listitems;


        adiLoading.IsRunning = false;
    }

    private void Unfollow_Invoked(object sender, EventArgs e)
    {

    }

    private void clvUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (clvUserList.SelectedItem != null)
        {
            UserListItem item = clvUserList.SelectedItem as UserListItem;
            clvUserList.SelectedItem = null;
            Shell.Current.GoToAsync("/user?id=" + item.UserID);
        }

    }
}