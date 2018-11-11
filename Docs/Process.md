# Journey of my first game jam

I started creating the game on Nov 5. I was playing a match-3 game and thinking to myself, these games have all these colorful tiles but they don't do anything with them. So I had the idea to create a matching-tile game about mixing colors.

## Nov 5

I have always been a fan of the hexagonal grid because they look cool. I want my game to use one too. I found some reference online about storing the coordinates and transferring between pixels and hexagonal coordinates. I quickly created my first prototype in several hours. 

I used the RGB color space in this version. I use the default additive shader from unity to create the color mixing effect. 

I found that match-3 rules on a hexagonal grid is too simple in that you can avoid mixing colors altogether and just put the primary tiles next to each other.  So I change the rules to formid primary colors from matching.

## Nov 6

I grabbed my roommate to test my game. He complained about the game being too hard for people who are not familiar with color theory although the preview of the resulted color helps. And I found him struggling with understanding where the pieces are or are not allowed to go. I decided to change up the rules.



## Nov 7-8

In an attemp to make the rules more elegant, I change the color space to red yellow and blue because 1. this is more inline with what we learnt when we were kids, red+yellow=orange, yellow+blue=green and blue+red=purple, and 2. it's also prettier in my mind. I also made the rules of the game such that there are small blobs of colors and larger ones. Two small ones create a big one and only the big ones match. Working in this way, it also gives me freedom to create much more colors.

I created the sprites in photoshop. I didn't know about spritesheet at this time so I sliced them into 20 or so pieces.

I added a rainbow bomb so that you can clean up the board when you are stuck. 

I invited more of my friends to play the game. They still find the mixing-rules a bit complicated if they are unfamiliar with colors. One of them suggests cuter graphics and maybe animation to make it more attracting. 

I also port the game to mobile to see how it works on a phone. Turns out that the controls are too small for fingers and the previews are block by the finger. 

I spend some time making the transition smoother and decided that I wanted to update the graphics.

## Nov 9

This is when I first seriously considered joining the game jam. I started thinking of ways to make my game more fitting to the theme. I thought of hybriding cute monsters (not the most inventive idea but still). so I sat down and drew up some animals for each color. I also gave them different amount of eyes to distinguish between primary, secondary and tertiary colors.