using System;
using System.Linq;
using ZappChat_v3.Controls;
using ZappChat_v3.Controls.MainContentControls;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    public partial class ChatWindowModel
    {
        private void BindCommandCallbacks()
        {
            CommandManager.CloseCurrentContent += () => MainContent = null;
            CommandManager.OpenGroupCreateCommand.Action += GroupCreateCommandCallback;
            CommandManager.GroupCreateCommand.ParameterizedAction += GroupCreateCommandCallback;
            CommandManager.OpenGroupSettingCommand.ParameterizedAction += OpenGroupSettingCommandCallback;
            CommandManager.GroupDeleteCommand.ParameterizedAction += GroupDeleteCommandCallback;
            CommandManager.AddFriendInGroupCommand.ParameterizedAction += AddFriendInGroupCommandCallback;
            CommandManager.AddFriendCommand.Action += AddFriendCommandCallback;
            CommandManager.OpenFriendChatCommand.ParameterizedAction +=OpenFriendChatCommandCallback;
            CommandManager.DeleteFriendCommand.ParameterizedAction +=DeleteFriendCommandOnParameterizedAction;
        }

        private void AddFriendCommandCallback()
        {
            var add = new TEST_addFriend(FriendCollection);
            add.ShowDialog();
            add.Close();
        }

        private void GroupCreateCommandCallback()
        {
            MainContent = new GroupCreate();
        }

        private void GroupCreateCommandCallback(object groupParam)
        {
            var group = groupParam as Group;
            DbContentManager.Instance.Groups.Add(group);
            DbContentManager.Instance.SaveChanges();
            GroupCollection.Add(group);
            MainContent = null;
        }

        private void OpenGroupSettingCommandCallback(object groupId)
        {
            if (groupId == null) throw new NullReferenceException();
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(groupId as string));
            MainContent = new GroupSettingContent(group);
        }

        private void GroupDeleteCommandCallback(object groupId)
        {
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(groupId as string));
            DbContentManager.Instance.Groups.Remove(group);
            DbContentManager.Instance.SaveChanges();
            GroupCollection.Remove(group);
            MainContent = null;
        }

        private void AddFriendInGroupCommandCallback(object groupId)
        {
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(groupId as string));
            var add = new TEST_addFriendInGroup(group);
            add.ShowDialog();
            add.Close();
        }

        private void OpenFriendChatCommandCallback(object friendParam)
        {
            var friend = friendParam as Friend;
            if(friend == null) throw new NullReferenceException("Не возможно открыть чат! Ссылка ссылается на null");
            MainContent = new FriendChatContent(friend.ChatMemberId);
        }

        private void DeleteFriendCommandOnParameterizedAction(object friendParam)
        {
            var friend = friendParam as Friend;
            var content = MainContent as FriendChatContent;
            if (content != null)
            {
                var chat = content;
                var chatFriend = chat.DataContext as Friend;
                if (chatFriend != null && chatFriend.Equals(friend))
                    MainContent = null;
            }
            FriendCollection.Remove(friend);
            DbContentManager.Instance.Friends.Remove(friend);
            DbContentManager.Instance.SaveChanges();
        }
    }
}