using System;
using System.Linq;
using ZappChat_v3.Controls.MainContentControls;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    public partial class ChatWindowModel
    {
        private OpenCommand _lastOpenCommand;

        private void BindCommandCallbacks()
        {
            CommandManager.GroupCreateCommand.Do += GroupCreateCommandCallback;
            CommandManager.GroupDeleteCommand.Do += GroupDeleteCommandCallback;
            CommandManager.AddFriendInGroupCommand.Do += AddFriendInGroupCommandCallback;
            CommandManager.AddFriendCommand.Do += AddFriendCommandCallback;
            CommandManager.DeleteFriendCommand.Do +=DeleteFriendCommandCallback;

            CommandManager.FriendChatOpenCommand.Do += FriendChatOpenCommandCallback;
            CommandManager.FriendChatOpenCommand.DoWhenClose += DefaultCloseCallback;
            CommandManager.GroupCreateOpenCommand.Do += GroupCreateOpenCommandCallback;
            CommandManager.GroupCreateOpenCommand.DoWhenClose += DefaultCloseCallback;
            CommandManager.GroupSettingOpenCommand.Do += GroupSettingOpenCommandCallback;
            CommandManager.GroupSettingOpenCommand.DoWhenClose += DefaultCloseCallback;
            CommandManager.SettingOpenCommand.Do += SettingOpenCommandCallback;
            CommandManager.SettingOpenCommand.DoWhenClose += SettingOpenCommandCloseCallback;
        }

        private void CloseCommand(OpenCommand currentOpenCommand)
        {
            _lastOpenCommand?.DoExecute(null, true);
            _lastOpenCommand = currentOpenCommand;
            _chatWindow.GuiReactionOnExecuteCommand(currentOpenCommand.Name);
        }

        private void GroupCreateCommandCallback(object groupParam)
        {
            var group = groupParam as Group;
            DbContentManager.Instance.Groups.Add(@group);
            DbContentManager.Instance.SaveChanges();
            GroupCollection.Add(@group);
            MainContent = null;
        }

        private void GroupDeleteCommandCallback(object groupId)
        {
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(groupId as string));
            DbContentManager.Instance.Groups.Remove(@group);
            DbContentManager.Instance.SaveChanges();
            GroupCollection.Remove(@group);
            MainContent = null;
        }

        private void AddFriendInGroupCommandCallback(object groupId)
        {
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(groupId as string));
            var add = new TEST_addFriendInGroup(@group);
            add.ShowDialog();
            add.Close();
        }

        private void AddFriendCommandCallback(object param)
        {
            var add = new TEST_addFriend(FriendCollection);
            add.ShowDialog();
            add.Close();
        }

        private void DeleteFriendCommandCallback(object friendParam)
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

        //--- OpenCommand callbacks ---

        private void FriendChatOpenCommandCallback(object friendParam)
        {
            CloseCommand(CommandManager.FriendChatOpenCommand);
            var friend = friendParam as Friend;
            if(friend == null) throw new NullReferenceException("Не возможно открыть чат! Ссылка ссылается на null");
            MainContent = new FriendChatContent(friend.ChatMemberId);

        }

        private void GroupCreateOpenCommandCallback(object param)
        {
            CloseCommand(CommandManager.GroupCreateOpenCommand);
            MainContent = new GroupCreate();
        }

        private void GroupSettingOpenCommandCallback(object groupId)
        {
            CloseCommand(CommandManager.GroupSettingOpenCommand);
            if (groupId == null) throw new NullReferenceException();
            var group = GroupCollection.First(g => g.ChatMemberId.Equals(groupId as string));
            MainContent = new GroupSettingContent(@group);
        }

        private void SettingOpenCommandCallback(object param)
        {
            var settingContent = new SettingsContent(MainContent);
            CloseCommand(CommandManager.SettingOpenCommand);
            MainContent = settingContent;
        }

        private void DefaultCloseCallback(object param)
        {
            MainContent = null;
        }

        private void SettingOpenCommandCloseCallback(object o)
        {  
            var lastContent = (o as SettingsContent)?.LastControl;
            MainContent = lastContent;
        }
    }
}