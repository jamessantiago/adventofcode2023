#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <regex.h>
#include <stdbool.h>

#define ARRAY_SIZE(x) (sizeof(x) / sizeof(x[0]))

int main()
{
  FILE *f = NULL;
  char buf[256];
  int day2a = 0;
  int day2b = 0;
  int game_i = 0;
  regex_t game_pattern, red_pattern, green_pattern, blue_pattern;
  regmatch_t game_match[1], color_match[2];
  char *token, *rest;
  char *token2, *rest2;

  f = fopen("02.txt", "r");
  if (f == NULL)
    goto done;

  regcomp(&game_pattern, "Game [0-9]+: ", REG_EXTENDED);
  regcomp(&red_pattern, "[0-9]+ red", REG_EXTENDED);
  regcomp(&green_pattern, "[0-9]+ green", REG_EXTENDED);
  regcomp(&blue_pattern, "[0-9]+ blue", REG_EXTENDED);

  while (fgets(buf, 256, f) != NULL)
  {
    game_i++;
    int min_game[3] = {0};
    regexec(&game_pattern, buf, ARRAY_SIZE(game_match), game_match, 0);
    char *game_set = buf + game_match[0].rm_eo;
    // printf("%s\n", game_set);
    bool is_possible = true;
    rest = game_set;
    while ((token = strtok_r(rest, ";", &rest)))
    {
      // printf("%s\n", token);
      rest2 = token;
      while ((token2 = strtok_r(rest2, ",", &rest2)))
      {
        if (regexec(&red_pattern, token2, ARRAY_SIZE(color_match), color_match, 0) == 0)
        {
          char *red_set = calloc(color_match[0].rm_eo, sizeof(int));
          strncpy(red_set, token2 + color_match[0].rm_so, color_match[0].rm_eo);
          int red = atoi(red_set);
          if (red > 12)
            is_possible = false;
          if (red > min_game[0])
            min_game[0] = red;
          free(red_set);
        }

        if (regexec(&green_pattern, token2, ARRAY_SIZE(color_match), color_match, 0) == 0)
        {
          char *green_set = calloc(color_match[0].rm_eo, sizeof(int));
          strncpy(green_set, token2 + color_match[0].rm_so, color_match[0].rm_eo);
          int green = atoi(green_set);
          if (green > 12)
            is_possible = false;
          if (green > min_game[1])
            min_game[1] = green;
          free(green_set);
        }

        if (regexec(&blue_pattern, token2, ARRAY_SIZE(color_match), color_match, 0) == 0)
        {
          char *blue_set = calloc(color_match[0].rm_eo, sizeof(int));
          strncpy(blue_set, token2 + color_match[0].rm_so, color_match[0].rm_eo);
          int blue = atoi(blue_set);
          if (blue > 12)
            is_possible = false;
          if (blue > min_game[2])
            min_game[2] = blue;
          free(blue_set);
        }
      }
    }
    // printf("Game %d: %s\n", game_i, is_possible ? "possible" : "impossible");
    // printf("Game %d: %d %d %d\n", game_i, min_game[0], min_game[1], min_game[2]);
    // printf("Game %d: %d\n", game_i, min_game[0] * min_game[1] * min_game[2]);
    day2a += is_possible ? game_i : 0;
    day2b += min_game[0] * min_game[1] * min_game[2];
  }

  printf("Day 02-A: %d\n", day2a);
  printf("Day 02-B: %d\n", day2b);

done:
  if (f)
    fclose(f);
}
