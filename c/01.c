#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main()
{
  FILE *f = NULL;
  char buf[50];
  int day1a = 0;
  int day1b = 0;

  f = fopen("01.txt", "r");
  if (f == NULL)
    goto done;

  while (fgets(buf, 50, f) != NULL)
  {
    int nums[50] = {0};
    for (int i = 0; i < strlen(buf); i++)
    {
      char c[2] = {0};
      sprintf(c, "%c", buf[i]);
      nums[i] = atoi(c);
    }
    int first = 0, last = 0;
    for (int i = 0; i < strlen(buf); i++)
    {
      if (nums[i] == 0)
        continue;
      if (first == 0)
        first = nums[i];
      last = nums[i];
    }
    char digits[3];
    sprintf(digits, "%d%d", first, last);
    // printf("%s\n", digits);
    int num = atoi(digits);
    day1a += num;
  }

  char *num_names[9] = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
  fseek(f, 0, SEEK_SET);
  while (fgets(buf, 50, f) != NULL)
  {
    int nums[50] = {0};
    for (int i = 0; i < strlen(buf); i++)
    {
      char c[2] = {0};
      sprintf(c, "%c", buf[i]);
      nums[i] = atoi(c);

      if (nums[i] == 0)
      {
        for (int j = 0; j < 9; j++)
        {
          char tmp[50] = {0};
          strncpy(tmp, &buf[i], strlen(num_names[j]));
          // printf("%s == %s\n", tmp, num_names[j]);
          if (strcmp(tmp, num_names[j]) == 0)
          {
            nums[i] = j + 1;
            break;
          }
        }
      }
    }
    int first = 0, last = 0;
    for (int i = 0; i < strlen(buf); i++)
    {
      if (nums[i] == 0)
        continue;
      if (first == 0)
        first = nums[i];
      last = nums[i];
    }
    char digits[3];
    sprintf(digits, "%d%d", first, last);
    printf("%s\n", digits);
    int num = atoi(digits);
    day1b += num;
  }

  printf("Day 01: %d\n", day1a);
  // wrong, but why?
  printf("Day 02: %d\n", day1b);

done:
  if (f)
    fclose(f);
}
