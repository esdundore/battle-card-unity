using System.Collections.Generic;

public static class MessageText
{

    public static Dictionary<int,string> messageMap = new Dictionary<int,string>(){
        {0, "" },
        {1, "Hello, I'm here to teach you how to play Battle Card. I will go first this match, but before I do, you can make up to two of your cards into a resource called GUTS. Just drag one of your cards over your avatar to transform it into GUTS!" },
        {2, "I can't use any of my attacks this turn, since I don't have enough GUTS to use them. You can see how many GUTS an attack costs by looking at the droplet icon in the bottom left corner of a skill card in your hand." },
        {3, "I will save one card and turn the rest into GUTS. Your turn!" },
        {4, "Now it's your turn. You can use your Headbutt skill this turn since you have enough GUTS. To use a skill, drag it to the member of your team who can use it." },
        {5, "Good! Now Mocchi is attacking with its Headbutt skill. You can choose a target for this attack by clicking and dragging the target icon to an enemy. Try targeting my Mocchi." },
        {6, "Not so fast! I will use Round in response to your attack. Round is a block skill that prevents damage from an attack. Now you have spent your GUTS on an attack, you will need to make more before attacking again. Press the button to continue to the GUTS phase." },
        {7, "You should save your defense skills, Endure and Jump Aside, but make the rest of your cards into GUTS" },
        {8, "Ok my turn. I'm attacking your Dino with Mocchi's Motch Ray. You can't use Endure to block damage from this attack since it is an INT attack, and not a POW attack. Try using Jump Aside to dodge it." },
        {9, "Good! Dino dodged the attack preventing damage, but I'm coming at you with another attack and it doesn't look like you have any defenses this time."},
        {10, "I'm done my turn and making these GUTS. Your turn." },
        {11, "Not all skills are attacks and defenses. Try using that Mango skill you just drew. Mango can be used by you to heal some life to monsters on your team. Click and drag the target to Tiger to restore the life points it lost." },
        {12, "Now let's try a combo. First drag the Left Claw card to Tiger. You can also drag Right Claw onto Tiger to combo it with Left Claw and deal bonus damage. Then drag the target icon to any of my monsters." },
        {13, "Well done! I think you've got the hang of this. Let's keep playing until one of us wins the game. To win, you must knock out all your opponent's monsters by reducing their life to 0. A player also loses if they don't have cards in their deck on their draw phase." }
    };

}
