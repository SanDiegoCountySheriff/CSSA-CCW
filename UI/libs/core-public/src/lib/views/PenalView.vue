<template>
  <div>
    <v-container>
      <v-card
        :color="$vuetify.theme.dark ? 'info' : 'accent'"
        :style="{ color: $vuetify.theme.dark ? '#FFF' : '#FFF' }"
      >
        <v-card-title>
          {{ $t(' Applicable California Penal Code Sections') }}
        </v-card-title>
      </v-card>
      <v-card class="text-left">
        <v-card-title class="font-weight-bold">
          {{
            $t(
              'The following Penal Code sections are of special importance to the holder of a CCW license regarding the use, carrying, and storage of firearms: '
            )
          }}
        </v-card-title>

        <v-divider />

        <v-card-title class="font-weight-bold">
          {{
            $t('Penal Code section 26180 - False Statement on Application Form')
          }}
        </v-card-title>

        <v-card-text>
          {{
            $t(
              '(a) Any person who files an application required by Section 26175 knowing that statements contained therein are false is guilty of a misdemeanor.'
            )
          }}
        </v-card-text>

        <v-card-text>
          {{
            $t(
              '(b) Any person who knowingly makes a false statement on the application regarding any of the following is guilty of a felony.'
            )
          }}
          <br />
          <span class="ml-5">{{
            $t(
              '(1) The denial or revocation of a license, or the denial of an amendment to a license, issued pursuant to this article.'
            )
          }}</span>
          <br />
          <span class="ml-5">{{ $t('(2) A criminal conviction.') }}</span>
          <br />
          <span class="ml-5">{{
            $t('(3) A finding of not guilty by reason of insanity.')
          }}</span>
          <br />
          <span class="ml-5">{{
            $t('(4) The use of a controlled substance.')
          }}</span>
          <br />
          <span class="ml-5">{{
            $t('(5) A dishonorable discharge from military service.')
          }}</span>
          <br />
          <span class="ml-5">{{
            $t('(6) A commitment to a mental institution.')
          }}</span>
          <br />
          <span class="ml-5">{{
            $t('(7) A renunciation of United States citizenship.')
          }}</span>
          <br />
        </v-card-text>

        <v-card-title class="font-weight-bold">
          {{ $t('Penal Code section 192 - Manslaughter') }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              'Manslaughter is the unlawful killing of a human being without malice.'
            )
          }}
          <br />
          {{ $t('(a) Voluntary - upon a sudden quarrel or heat of passion.') }}
          <br />
          {{
            $t(
              '(b) Involuntary - in the commission of an unlawful act, not amounting to a felony; or in the commission of a lawful act which might produce death, in an unlawful manner, or without due caution and circumspection. This subdivision shall not apply to acts committed in the driving of a vehicle.'
            )
          }}
        </v-card-text>

        <v-card-title class="font-weight-bold">
          {{ $t('Penal Code section 197 - Justifialbe Homicide; Any Person') }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              'Homicide is also justifiable when committed by any person in any of the following cases:'
            )
          }}
          <br />
          <span class="ml-5">{{
            $t(
              '1. When resisting any attempt to murder any person, or to commit a felony, or to do some great bodily injury upon any person; or, '
            )
          }}</span>
          <br />
          <span class="ml-5">{{
            $t(
              '2. When committed in defense of habitation, property, or person, against one who manifestly intends or endeavors, by violence or surprise, to commit a felony, or against one who manifestly intends and endeavors, in a violent, riotous or tumultuous manner, to enter the habitation of another for the purpose of offering violence to any person therein; or'
            )
          }}</span>
          <br />
          <span class="ml-5">{{
            $t(
              '3. When committed in the lawful defense of such person, or of a wife or husband, parent, child, master, mistress or servant of such person, when there is reasonable ground to apprehend a design to commit a felony or to do some great bodily injury, and imminent danger of such design being accomplished; but such person, or the person in whose behalf the defense was made, if he was the assailant or engaged in mutual combat, must really and in good faith have endeavored to decline any further struggle before the homicide was committed; or'
            )
          }}</span>
          <br />
          <span class="ml-5">{{
            $t(
              '4. When necessarily committed in attempting, by lawful ways and means, to apprehend any person for any felony committed, or in lawfully suppressing any riot, or in lawfully keeping and preserving the peace.'
            )
          }}</span>
          <br />
        </v-card-text>

        <v-card-title class="font-weight-bold">
          {{
            $t(
              'Penal Code sectoin 198 - Justifialbe Homicide; Sufficiency of Fear'
            )
          }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              'A bare fear of the commission of any of the offenses mentioned in subdivisions 2 and 3 of Section 197, to prevent which homicide may be lawfully committed, is not sufficient to justify it. But the circumstances must be sufficient to excite the fears of a reasonable person, and the party killing must have acted under the influence of such fears alone.'
            )
          }}
        </v-card-text>

        <v-card-title class="font-weight-bold">
          {{
            $t(
              'Penal Code section 199 - Justifiable and Excusable Homicide; Discharge of Defendant'
            )
          }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              'The homicide appearing to be justifiable or excusable, the person indicted must, upon his trial, be fully acquitted and discharged.'
            )
          }}
        </v-card-text>
        <v-card-title class="font-weight-bold">
          {{ $t('Penal Code section 25100 - Criminal Storage of Firearm') }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              '(a) Except as provided in Section 25105, a person commits the crime of "criminal storage of a firearm of the first degree" if all of the following conditions are satisfied.'
            )
          }}
          <br />
          <span class="ml-5">
            {{
              $t(
                "(1) The person keeps any loaded firearm within any premises that are under the person's custody or control."
              )
            }}
          </span>
          <br />
          <span class="ml-5">
            {{
              $t(
                "(2) The person knows or reasonably should know that a child is likely to gain access to the firearm without the permission of the child's parent or legal guardian."
              )
            }}
          </span>
          <br />
          <span class="ml-5">
            {{
              $t(
                '(3) The child obtains access to the firearm and thereby causes death or great bodily injury to the child or any other person.'
              )
            }}
          </span>
          {{
            $t(
              '(b) Except as provided in Section 25105, a person commits the crime of "criminal storage of a firearm of the second degree" if all of the following conditions are satisfied:'
            )
          }}
          <br />
          <span>
            {{
              $t(
                "(1) The person keeps any loaded firearm within any premises that are under the person's custody or control."
              )
            }}
          </span>
          <br />
          <span>
            {{
              $t(
                "(2) The person knows or reasonably should know that a child is likely to gain access to the firearm without the permission of the child's parent or legal guardian."
              )
            }}
          </span>
          <br />
          <span>
            {{
              $t(
                '(3) The child obtains access to the firearm and thereby causes injury, other than great bodily injury, to the child or any other person, or carries the firearm either to a public place or in violation of Section 417.'
              )
            }}
          </span>
          <br />
        </v-card-text>
        <v-card-title class="font-weight-bold">
          {{ $t('Penal Code section 25105 - Exceptions') }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              'Section 25100 does not apply whenever any of the following occurs:'
            )
          }}
          <br />
          {{
            $t(
              '(a) The child obtains the firearm as a result of an illegal entry to any premises by any person.'
            )
          }}
          <br />
          {{
            $t(
              '(b) The firearm is kept in a locked container or in a location that a reasonable person would believe to be secure.'
            )
          }}
          <br />
          {{
            $t(
              '(c) The firearm is carried on the person or within close enough proximity thereto that the individual can readily retrieve and use the firearm as if carried on the person.'
            )
          }}
          <br />
          {{
            $t(
              '(d) The firearm is locked with a locking device, as defined in Section 16860, which has rendered the firearm inoperable.'
            )
          }}
          <br />
          {{
            $t(
              "(e) The person is a peace officer or a member of the Armed Forces or the National Guard and the child obtains the firearm during, or incidental to, the performance of the person's duties."
            )
          }}
          <br />
          {{
            $t(
              '(f) The child obtains, or obtains and discharges, the firearm in a lawful act of self-defense for defense of another person.'
            )
          }}
          <br />
          {{
            $t(
              "(g) The person who keeps a loaded firearm on any premise that is under the person's custody or control has no reasonable expectation, based on objective facts and circumstances, that a child is likely to be present on the premise."
            )
          }}
          <br />
        </v-card-text>
        <v-card-title class="font-weight-bold">
          {{
            $t(
              'Penal Code sectoin 23200 - Storage of Firearm where Child Obtains Access and Carries Firearm Off-Premises'
            )
          }}
        </v-card-title>
        <v-card-text>
          {{
            $t(
              '(a) If all of the following conditions are satisfied, a person shall be punished by imprisonment in a county jail not exceeding one year, by a fine not exceeding one thousand dollars ($1,000), or by both that imprisonment and fine: '
            )
          }}
          <br />
          <span class="ml-5">
            {{
              $t(
                "(1) The person keeps a pistol, revolver, or other firearm capable of being concealed upon the person, loaded or unloaded, within any premises that are under the person's custody or control."
              )
            }}
          </span>

          <span class="ml-5">
            {{
              $t(
                "(2) The person knows or reasonably should know that a child is likely to gain access to that firearm without the permission of the child's parent or legal guardian."
              )
            }}
          </span>
          <br />

          <span class="ml-5">
            {{
              $t(
                '(3) The child obtains access to that firearm and thereafter carries that firearm off-premises.'
              )
            }}
          </span>
          <br />
          {{
            $t(
              '(b) If all of the following conditions are satisfied, a person shall be punished by imprisonment in a county jail not exceeding one year, by a fine not exceeding five thousand dollars ($5,000), or by both that imprisonment and fine:'
            )
          }}

          <span class="ml-5">
            {{
              $t(
                "(1) The person keeps any firearm within any premises that are under the person's custody or control."
              )
            }}
          </span>
          <br />

          <span class="ml-5">
            {{
              $t(
                "(2) The person knows or reasonably should know that a child is likely gain access to the firearm without the permission of the child's parent or legal guardian."
              )
            }}
          </span>
          <br />

          <span class="ml-5">
            {{
              $t(
                '(3) The child obtains access to the firearm and thereafter carries that firearm off-premises to any public or private preschool, elementary school, middle school, high school, or to any school-sponsored event, activity, or performance, whether occurring on school grounds or elsewhere.'
              )
            }}
          </span>
          <br />
          {{
            $t(
              '(c) A pistol, revolver, or other firearm capable of being concealed upon the person that a child gains access to and carries off-premises in violation of this section shall be deemed "used in the commission of any misdemeanor as provided in this code or any felony" for the purpose of Section 29300 regarding the authority to confiscate firearms and other deadly weapons as a nuisance. '
            )
          }}
          <br />

          {{
            $t(
              '(d) As used in this section, "off-premises" means premises other than the premises where the firearm was stored.'
            )
          }}
          <br />
        </v-card-text>

        <v-card-title class="font-weight-bold">
          {{ $t('Penal Code section 25205 - Exceptions') }}
        </v-card-title>
        <v-card-text>
          {{
            $t('Section 25200 does not apply if any of the following are true:')
          }}
          <br />
          {{
            $t(
              '(a) The child obtains the firearm as a result of an illegal entry into any premises by any person.'
            )
          }}
          <br />
          {{
            $t(
              '(b) The firearm is kept in a locked container or in a location that a reasonable person would believe to be secure.'
            )
          }}
          <br />

          {{
            $t(
              '(c) The firearm is locked with a locking device, as defined in Section 16860, which has rendered the firearm inoperable.'
            )
          }}
          <br />

          {{
            $t(
              '(d) The firearm is carried on the person within close enough range that the individual can readily retrieve and use the firearm as if carried on the person.'
            )
          }}
          <br />

          {{
            $t(
              "(e) The person is a peace officer or a member of the Armed Forces or National Guard and the child obtains the firearm during, or incidental to, the performance of the person's duties."
            )
          }}
          <br />

          {{
            $t(
              '(f) The child obtains, or obtains and discharges, the firearm in a lawful act of self-defense or defense of another person.'
            )
          }}
          <br />

          {{
            $t(
              '(g) The person who keeps a firearm has no reasonable expectation, based on objective facts and circumstances, that a child is likely to be present on the premises.'
            )
          }}
          <br />
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup lang="ts"></script>
