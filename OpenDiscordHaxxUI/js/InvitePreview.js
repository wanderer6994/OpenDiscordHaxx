const GuildVerificationLevel = {
    None: 0,
    Low: 1,
    Medium: 2,
    High: 3,
    VeryHigh: 4
}


function HandleInvite(data) {
    data = JSON.parse(data);

    document.getElementById('invite-preview').style.display = '';
    const guildPreview = document.getElementById('guild-invite-preview');
    const groupPreview = document.getElementById('group-invite-preview');

    if (data.guild) {
        guildPreview.style.display = '';
        groupPreview.style.display = 'none';

        document.getElementById('guild-preview-name').innerText = 'Name: ' + data.guild.name;
        document.getElementById('guild-preview-verification').innerText = 'Verification level: ' + GetVerificationLevel(data.guild);
        document.getElementById('guild-preview-members').innerText = 'Members: ' + data.approximate_member_count;
    }
    else {
        guildPreview.style.display = 'none';
        groupPreview.style.display = '';

        document.getElementById('group-preview-name').innerText = 'Name: ' + data.channel.name;
        document.getElementById('group-preview-recipients').innerText = 'Recipients: ' + data.approximate_member_count;
        document.getElementById('group-preview-inviter').innerHTML = 'Inviter: ' + data.inviter.username
                                                                     + '<span style="color: rgb(200,202,205); margin-left: 2px">#' + data.inviter.discriminator + '</span>';
    }
}


function GetVerificationLevel(guild) {
    switch (guild.verification_level) {
        case GuildVerificationLevel.None:
            return 'None';
        case GuildVerificationLevel.low:
            return 'Low';
        case GuildVerificationLevel.Medium:
            return 'Medium';
        case GuildVerificationLevel.High:
            return 'High';
        case GuildVerificationLevel.VeryHigh:
            return 'Very high';
    }
}