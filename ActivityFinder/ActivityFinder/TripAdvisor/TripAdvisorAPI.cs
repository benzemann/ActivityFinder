using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using log4net;
using ActivityModel;
using System.Text.RegularExpressions;

namespace ActivityFinder.TripAdvisor
{
    public static class TripAdvisorAPI
    {

        private static readonly ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region City urls
        private static string[] cities = new string[] {
            /*"/Attractions-g227593-Activities-[PAGE]Horsens_East_Jutland_Jutland.html",
            "/Attractions-g230034-Activities-[PAGE]Skive_West_Jutland_Jutland.html",
            "/Attractions-g1370488-Activities-[PAGE]Holbaek_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g1102747-Activities-[PAGE]Skanderborg_East_Jutland_Jutland.html",
            "/Attractions-g226901-Activities-[PAGE]Herning_West_Jutland_Jutland.html",*/
            "/Attractions-g227599-Activities-[PAGE]Kolding_South_Jutland_Jutland.html",
            /*"/Attractions-g226902-Activities-[PAGE]Silkeborg_East_Jutland_Jutland.html",
            "/Attractions-g226904-Activities-[PAGE]Vejle_South_Jutland_Jutland.html",
            "/Attractions-g189521-Activities-[PAGE]Langeland_Funen_and_Islands.html",
            "/Attractions-g1601857-Activities-[PAGE]Nykoebing_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g189528-Activities-[PAGE]Aabenraa_South_Jutland_Jutland.html",
            "/Attractions-g1222129-Activities-[PAGE]Lemvig_West_Jutland_Jutland.html",
            "/Attractions-g1069587-Activities-[PAGE]Kerteminde_Funen_and_Islands.html",
            "/Attractions-g667542-Activities-[PAGE]Naestved_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555539-Activities-[PAGE]Kastrup_Taarnby_Municipality_Copenhagen_Region_Zealand.html",*/
            "/Attractions-g189544-Activities-[PAGE]Roskilde_West_Zealand_Zealand.html",
            /*"/Attractions-g659281-Activities-[PAGE]Struer_West_Jutland_Jutland.html",
            "/Attractions-g285705-Activities-[PAGE]Skagen_North_Jutland_Jutland.html",
            "/Attractions-g1207777-Activities-[PAGE]Assens_Funen_and_Islands.html",
            "/Attractions-g230030-Activities-Fredericia_South_Jutland_Jutland.html",
            "/Attractions-g227595-Activities-Ringsted_Ringsted_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2549257-Activities-Hedensted_East_Jutland_Jutland.html",
            "/Attractions-g1173295-Activities-Odder_East_Jutland_Jutland.html",
            "/Attractions-g189523-Activities-Nyborg_Funen_and_Islands.html",
            "/Attractions-g189516-Activities-Falster_South_Zealand_Zealand.html",
            "/Attractions-g2549749-Activities-Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g1096218-Activities-Faaborg_Faaborg_MidtFunen_Municipality_Funen_and_Islands.html",
            "/Attractions-g230033-Activities-Ringkobing_West_Jutland_Jutland.html",
            "/Attractions-g189542-Activities-Helsingoer_Helsingoer_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1968265-Activities-Vejen_South_Jutland_Jutland.html",
            "/Attractions-g227592-Activities-Grenaa_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g189538-Activities-Moen_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g608737-Activities-Soroe_West_Zealand_Zealand.html",
            "/Attractions-g670016-Activities-Samsoe_Jutland.html",
            "/Attractions-g189519-Activities-Aero_Funen_and_Islands.html",
            "/Attractions-g230028-Activities-Ebeltoft_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1189043-Activities-Loegstoer_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2550854-Activities-Skoerping_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2550849-Activities-Hadsund_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g1102339-Activities-Laesoe_Island_North_Jutland_Jutland.html",
            "/Attractions-g189533-Activities-Fanoe_South_Jutland_Jutland.html",
            "/Attractions-g1370489-Activities-Hundested_Halsnaes_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2050142-Activities-Skjern_West_Jutland_Jutland.html",
            "/Attractions-g189522-Activities-Middelfart_Funen_and_Islands.html",
            "/Attractions-g230032-Activities-Koege_Koege_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1173294-Activities-Brovst_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g635556-Activities-Kalundborg_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1945872-Activities-Tranekaer_Langeland_Funen_and_Islands.html",
            "/Attractions-g189531-Activities-Billund_South_Jutland_Jutland.html",
            "/Attractions-g189536-Activities-Nakskov_Lolland_South_Zealand_Zealand.html",*/
            "/Attractions-g189541-Activities-[PAGE]Copenhagen_Zealand.html",
            /*"/Attractions-g189530-Activities-Aarhus_East_Jutland_Jutland.html",
            "/Attractions-g189529-Activities-Aalborg_North_Jutland_Jutland.html",
            "/Attractions-g189524-Activities-Odense_Funen_and_Islands.html",
            "/Attractions-g230035-Activities-Slagelse_West_Zealand_Zealand.html",
            "/Attractions-g230037-Activities-Viborg_East_Jutland_Jutland.html",
            "/Attractions-g189534-Activities-Lolland_South_Zealand_Zealand.html",
            "/Attractions-g681245-Activities-Hjorring_North_Jutland_Jutland.html",
            "/Attractions-g189513-Activities-Bornholm.html",
            "/Attractions-g189525-Activities-Svendborg_Funen_and_Islands.html",
            "/Attractions-g227598-Activities-Frederikshavn_North_Jutland_Jutland.html",
            "/Attractions-g189532-Activities-Esbjerg_South_Jutland_Jutland.html",
            "/Attractions-g227590-Activities-Varde_South_Jutland_Jutland.html",
            "/Attractions-g774853-Activities-Toender_South_Jutland_Jutland.html",
            "/Attractions-g616082-Activities-Thisted_North_Jutland_Jutland.html",
            "/Attractions-g230031-Activities-Holstebro_West_Jutland_Jutland.html",
            "/Attractions-g1370021-Activities-Haderslev_South_Jutland_Jutland.html",
            "/Attractions-g1222128-Activities-Bronderslev_North_Jutland_Jutland.html",
            "/Attractions-g227597-Activities-Soenderborg_South_Jutland_Jutland.html",
            "/Attractions-g227594-Activities-Randers_East_Jutland_Jutland.html",
            "/Attractions-g1370001-Activities-Farsoe_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g1369999-Activities-Brande_Ikast_Brande_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g1699896-Activities-Knebel_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1193035-Activities-Gilleleje_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g189535-Activities-Maribo_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2549134-Activities-Rudkoebing_Langeland_Funen_and_Islands.html",
            "/Attractions-g1572424-Activities-Slangerup_Alleroed_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g189539-Activities-Stege_Moen_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g189514-Activities-Roenne_Bornholm.html",
            "/Attractions-g1192991-Activities-Nexoe_Bornholm.html",
            "/Attractions-g189520-Activities-Bogense_North_Funen_Municipality_Funen_and_Islands.html",
            "/Attractions-g1949996-Activities-Gudhjem_Bornholm.html",
            "/Attractions-g656486-Activities-Fjerritslev_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2556108-Activities-Store_Heddinge_Stevns_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g667535-Activities-Haslev_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2550830-Activities-Erslev_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g2555517-Activities-Helsinge_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1370492-Activities-Hornbaek_Helsingoer_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1571711-Activities-Frederiksvaerk_Halsnaes_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g806258-Activities-Aars_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g1959809-Activities-Otterup_North_Funen_Municipality_Funen_and_Islands.html",
            "/Attractions-g1184821-Activities-Allinge_Bornholm.html",
            "/Attractions-g189515-Activities-Svaneke_Bornholm.html",
            "/Attractions-g2555565-Activities-Goerlev_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1808626-Activities-Hvide_Sande_West_Jutland_Jutland.html",
            "/Attractions-g800487-Activities-Aeroskobing_Aero_Funen_and_Islands.html",
            "/Attractions-g1601814-Activities-Roende_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1942915-Activities-Faxe_Ladeplads_Faxe_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555509-Activities-Graested_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g189543-Activities-Hillerod_Hilleroed_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g656485-Activities-Blokhus_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g1175484-Activities-Nykobing_Mors_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g2555975-Activities-Nysted_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g1966258-Activities-Akirkeby_Bornholm.html",
            "/Attractions-g227591-Activities-Glostrup_Broendby_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2477403-Activities-Hvalpsund_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g1549886-Activities-Tisvildeleje_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g729759-Activities-Roedby_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2554609-Activities-Jaegerspris_Frederikssund_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554666-Activities-Asnaes_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554676-Activities-Fuglebjerg_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g670018-Activities-Gedser_Falster_South_Zealand_Zealand.html",
            "/Attractions-g1997180-Activities-Skibby_Frederikssund_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555529-Activities-Vaerloese_Furesoe_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554601-Activities-Tarm_West_Jutland_Jutland.html",
            "/Attractions-g227588-Activities-Ikast_Ikast_Brande_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g2407736-Activities-Saltum_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g790588-Activities-Hoersholm_Hoersholm_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2431608-Activities-Aalestrup_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g189537-Activities-Sakskoebing_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2555548-Activities-Haarlev_Stevns_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555545-Activities-Noerre_Alslev_Falster_South_Zealand_Zealand.html",
            "/Attractions-g2554679-Activities-Horslunde_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2262789-Activities-Ranum_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g1601858-Activities-Roervig_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g1929594-Activities-Vester_Skerninge_Faaborg_MidtFunen_Municipality_Funen_and_Islands.html",
            "/Attractions-g806259-Activities-Vordingborg_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g608738-Activities-Charlottenlund_Gentofte_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1207781-Activities-Greve_Greve_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1966259-Activities-Borre_Moen_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555551-Activities-Herlufmagle_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2553053-Activities-Noerager_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2549104-Activities-Broby_Faaborg_MidtFunen_Municipality_Funen_and_Islands.html",
            "/Attractions-g2549131-Activities-Humble_Langeland_Funen_and_Islands.html",
            "/Attractions-g2213348-Activities-Klampenborg_Lyngby_Taarbak_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1207783-Activities-Ishoej_Ishoej_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1193022-Activities-Hasle_Bornholm.html",
            "/Attractions-g2483875-Activities-Havnebyen_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554675-Activities-Karrebaeksminde_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2549079-Activities-Soeby_Aero_Funen_and_Islands.html",
            "/Attractions-g2555819-Activities-Jyderup_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2550817-Activities-Pandrup_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2550828-Activities-Easter_Assels_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g2360337-Activities-Faarevejle_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2479713-Activities-Fur_West_Jutland_Jutland.html",
            "/Attractions-g2394784-Activities-Nimtofte_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g230029-Activities-Farum_Furesoe_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2556090-Activities-Glumsoe_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2475157-Activities-Allingaabro_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2554596-Activities-Hemmet_West_Jutland_Jutland.html",
            "/Attractions-g2554603-Activities-Videbaek_West_Jutland_Jutland.html",
            "/Attractions-g775899-Activities-Kongens_Lyngby_Lyngby_Taarbak_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g800486-Activities-Roedovre_Roedovre_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2540350-Activities-Hesselager_Funen_and_Islands.html",
            "/Attractions-g790323-Activities-Dragoer_Taarnby_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1370478-Activities-Hoerve_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g1175485-Activities-Guldborg_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2555852-Activities-Lejre_Lejre_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2552781-Activities-Moerke_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g226900-Activities-Ballerup_Ballerup_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g656487-Activities-Fredensborg_Fredensborg_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555995-Activities-Torrig_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2555554-Activities-Holmegaard_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555542-Activities-Eskilstrup_Falster_South_Zealand_Zealand.html",
            "/Attractions-g2554593-Activities-Toerring_Ikast_Brande_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g1820371-Activities-Roedvig_Stevns_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2554664-Activities-Grevinge_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554680-Activities-Holeby_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g1173293-Activities-Broendby_Broendby_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2550646-Activities-Glesborg_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2550643-Activities-Auning_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2549154-Activities-Aarup_North_Funen_Municipality_Funen_and_Islands.html",
            "/Attractions-g910730-Activities-Holte_Rudersdal_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2549164-Activities-Soendersoe_North_Funen_Municipality_Funen_and_Islands.html",
            "/Attractions-g1571712-Activities-Hellerup_Gentofte_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2550670-Activities-Kolind_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2549129-Activities-Bagenkop_Langeland_Funen_and_Islands.html",
            "/Attractions-g2555880-Activities-Dannemare_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2553379-Activities-Hadsten_Favrskov_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1096223-Activities-Valby_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2550843-Activities-Baelum_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2555980-Activities-Toreby_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2555990-Activities-Soellested_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g1966265-Activities-Foellenslev_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554597-Activities-Oelgod_West_Jutland_Jutland.html",
            "/Attractions-g2554595-Activities-Boevlingbjerg_Lemvig_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g2554607-Activities-Stenloese_Egedal_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554604-Activities-Lynge_Alleroed_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554662-Activities-Vig_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554674-Activities-Sandved_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1601840-Activities-Arden_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2052444-Activities-Jystrup_Ringsted_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g1601859-Activities-Hoejby_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g1370480-Activities-Herlev_Ballerup_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555827-Activities-Sejeroe_Island_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2550675-Activities-Oersted_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2552778-Activities-Hornslet_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1222176-Activities-Vallensbaek_Strand_Vallensbaek_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555555-Activities-Karise_Faxe_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555562-Activities-Hedehusene_Hoeje_Taastrup_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g729758-Activities-Niva_Fredensborg_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g230036-Activities-Taastrup_Hoeje_Taastrup_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555546-Activities-Stubbekoebing_Falster_South_Zealand_Zealand.html",
            "/Attractions-g2554602-Activities-Tim_West_Jutland_Jutland.html",
            "/Attractions-g2554600-Activities-Spjald_West_Jutland_Jutland.html",
            "/Attractions-g8078978-Activities-Nordby_Fanoe_South_Jutland_Jutland.html",
            "/Attractions-g1601853-Activities-Ringe_Faaborg_MidtFunen_Municipality_Funen_and_Islands.html",
            "/Attractions-g2480046-Activities-Liseleje_Halsnaes_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555837-Activities-Sveboelle_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2550684-Activities-Ryomgaard_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2555803-Activities-Hoeng_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555876-Activities-Bandholm_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g1931733-Activities-Praestoe_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2555854-Activities-Lille_Skensved_Koege_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1096220-Activities-Kongerslev_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2550832-Activities-Karby_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g2555967-Activities-Fejoe_Island_South_Zealand_Zealand.html",
            "/Attractions-g2555961-Activities-Errindlev_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2555500-Activities-Espergaerde_Helsingoer_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2296430-Activities-Olstykke_Egedal_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555969-Activities-Kettinge_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2550856-Activities-Terndrup_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2555558-Activities-Roennede_Faxe_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1439382-Activities-Hvidovre_Broendby_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1966264-Activities-Eskebjerg_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2556130-Activities-Toelloese_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554611-Activities-Bagsvaerd_Gladsaxe_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2553042-Activities-Gedsted_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2554663-Activities-Noerre_Asmindrup_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554672-Activities-Askeby_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2479975-Activities-Rude_Naestved_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2266950-Activities-Ostermarie_Bornholm.html",
            "/Attractions-g6495493-Activities-Nykobing_Falster_Falster_South_Zealand_Zealand.html",
            "/Attractions-g2549157-Activities-Morud_North_Funen_Municipality_Funen_and_Islands.html",
            "/Attractions-g2550688-Activities-Trustrup_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1571714-Activities-Birkerod_Furesoe_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g3252628-Activities-Suldrup_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g3252614-Activities-Klemensker_Bornholm.html",
            "/Attractions-g3252618-Activities-Borup_Koege_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2543599-Activities-Vaeggerlose_Falster_South_Zealand_Zealand.html",
            "/Attractions-g1219535-Activities-Marstal_Aero_Funen_and_Islands.html",
            "/Attractions-g680939-Activities-Humlebaek_Fredensborg_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2552775-Activities-Balle_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2550737-Activities-Jerslev_North_Jutland_Jutland.html",
            "/Attractions-g2550826-Activities-Vadum_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2550836-Activities-Redsted_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g2555511-Activities-Kvistgaard_Helsingoer_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1572420-Activities-Bronshoej_Copenhagen_Region_Zealand.html",
            "/Attractions-g226903-Activities-Vedbaek_Hoersholm_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1193032-Activities-Roemoe_Kirkeby_Roemoe_Toender_South_Jutland_Jutland.html",
            "/Attractions-g2555559-Activities-Tureby_Koege_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g729757-Activities-Rungsted_Hoersholm_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g729756-Activities-Kokkedal_Fredensborg_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555524-Activities-Skaevinge_Hilleroed_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1966267-Activities-Hvalsoe_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g812192-Activities-Marup_Samsoe_Jutland.html",
            "/Attractions-g2555543-Activities-Horbelev_Falster_South_Zealand_Zealand.html",
            "/Attractions-g1189039-Activities-Sandvig_Bornholm.html",
            "/Attractions-g8341689-Activities-Koge_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554592-Activities-Noerre_Snede_Ikast_Brande_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g4226697-Activities-Soborg_Gladsaxe_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554613-Activities-Melby_Halsnaes_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2554608-Activities-Veksoe_Egedal_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2556097-Activities-Lundby_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1820407-Activities-Bogoe_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2554671-Activities-Kalvehave_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2590014-Activities-Endelave_Island_East_Jutland_Jutland.html",
            "/Attractions-g2554681-Activities-Harpelunde_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2549099-Activities-Aarslev_Faaborg_MidtFunen_Municipality_Funen_and_Islands.html",
            "/Attractions-g2549160-Activities-Skamby_North_Funen_Municipality_Funen_and_Islands.html",
            "/Attractions-g3203519-Activities-Rudkobing_Langeland_Funen_and_Islands.html",
            "/Attractions-g2555839-Activities-Bjaeverskov_Koege_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2480002-Activities-Dronningmoelle_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2550683-Activities-Oerum_Djurs_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2480103-Activities-Vejby_Gribskov_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555842-Activities-Herfoelge_Koege_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2470351-Activities-Bork_Havn_West_Jutland_Jutland.html",
            "/Attractions-g2553383-Activities-Ulstrup_Favrskov_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g2543658-Activities-Store_Fuglede_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2470441-Activities-Thorup_Strand_Vesthimmerland_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2201650-Activities-Naerum_Rudersdal_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2550841-Activities-Vils_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g2475008-Activities-Skallerup_Morsoe_North_Jutland_Jutland.html",
            "/Attractions-g1193001-Activities-Balka_Bornholm.html",
            "/Attractions-g1193006-Activities-Sondervig_West_Jutland_Jutland.html",
            "/Attractions-g3660296-Activities-Keldby_Moen_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2479724-Activities-Gjol_Jammerbugt_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2555505-Activities-Hellebaek_Helsingoer_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g1193029-Activities-Dueodde_Bornholm.html",
            "/Attractions-g1572426-Activities-Snekkersten_Helsingoer_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555989-Activities-Easter_Ulslev_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2555986-Activities-Noerreballe_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g812190-Activities-Tranebjerg_Samsoe_Jutland.html",
            "/Attractions-g1968264-Activities-Tappernoeje_Faxe_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g812197-Activities-Oerby_Samsoe_Jutland.html",
            "/Attractions-g2555530-Activities-Virum_Lyngby_Taarbak_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g812195-Activities-Onsbjerg_Samsoe_Jutland.html",
            "/Attractions-g2477201-Activities-Ordrup_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2555544-Activities-Idestrup_Falster_South_Zealand_Zealand.html",
            "/Attractions-g2554598-Activities-Oernhoej_West_Jutland_Jutland.html",
            "/Attractions-g2556134-Activities-Ugerloese_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554594-Activities-Baekmarksbro_Lemvig_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g2554606-Activities-Smoerum_Egedal_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2556140-Activities-Kirke_Hyllinge_Lejre_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2554605-Activities-Broendy_Strand_Broendby_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2556136-Activities-Vipperoed_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2556146-Activities-Kirke_Saaby_Lejre_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2556119-Activities-Moerkoev_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554591-Activities-Bording_Ikast_Brande_Municipality_West_Jutland_Jutland.html",
            "/Attractions-g2556122-Activities-Nyrup_Odsherred_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554670-Activities-Mern_Vordingborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2554668-Activities-Stenlille_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554669-Activities-Svinninge_Holbaek_Municipality_West_Zealand_Zealand.html",
            "/Attractions-g2554678-Activities-Stokkemarke_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2554677-Activities-Vesterborg_Lolland_South_Zealand_Zealand.html",
            "/Attractions-g2554673-Activities-Klippinge_Stevns_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1601846-Activities-Femmoeller_Southdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g7340870-Activities-Refsvindinge_Funen_and_Islands.html",
            "/Attractions-g1601839-Activities-Rebild_Rebild_Municipality_North_Jutland_Jutland.html",
            "/Attractions-g2550640-Activities-Anholt_Northdjurs_Municipality_East_Jutland_Jutland.html",
            "/Attractions-g1571707-Activities-Karlslunde_Greve_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2204502-Activities-Oesterbro_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555812-Activities-Jerslev_Zealand_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g3203511-Activities-Lindelse_Langeland_Funen_and_Islands.html",
            "/Attractions-g2480044-Activities-Kulhuse_Frederikssund_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g3136949-Activities-Vestero_Havn_Laesoe_Island_North_Jutland_Jutland.html",
            "/Attractions-g2480059-Activities-Oelsted_Halsnaes_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2555833-Activities-Snertinge_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g2480072-Activities-Reersoe_Kalundborg_Municipality_South_Zealand_Zealand.html",
            "/Attractions-g1207779-Activities-Albertslund_Broendby_Municipality_Copenhagen_Region_Zealand.html",
            "/Attractions-g2480089-Activities-Stroeby_Stevns_Municipality_South_Zealand_Zealand.html"*/
        };
        #endregion

        public static void GetAllActivities(List<Activity> activities)
        {
            int i = 0;
            foreach(var city in cities)
            {
                log.Debug($"Getting activities from city url: {city}, progess: {i++} out of {cities.Count()}");
                try
                {
                    GetAllActivities(activities, city);
                }
                catch (Exception e)
                {
                    log.Error($"Failed to get activities from city url: {city}. \n Message: {e.Message} \n StackTrace: {e.StackTrace}");
                }
            }

        }

        public static void GetAllActivities(List<Activity> activities, string cityUrl)
        {
            log.Debug($"GetAllActivities: Getting activities from tripadvisor. City Url: {"https://www.tripadvisor.dk" + cityUrl.Replace("[PAGE]", "")}");

            var web = new HtmlWeb();
            var document = web.Load("https://www.tripadvisor.dk" + cityUrl.Replace("[PAGE]", ""));
            var currentOffset = 0;

            var page = document.DocumentNode.SelectNodes("//*[@id=\"FILTERED_LIST\"]/div[34]/div/div/div");
            var lastStr = page.Descendants()
                .Where(x => x.Attributes["Class"] != null && x.Attributes["Class"].Value == "pageNum taLnk")
                .Select(x => x.Attributes["data-offset"].Value).Last();
            var lastOffset = 0;

            if (!int.TryParse(lastStr, out lastOffset))
            {
                log.Error("Could not parse page numbers!");
                return;
            }

            var allTripAdvisorObjs = new List<dynamic>();

            while (currentOffset <= lastOffset)
            {

                allTripAdvisorObjs.AddRange(GetActivitiesFromPage(currentOffset, cityUrl));

                currentOffset += 30;
            }
            int i = 0;
            foreach (var tripAdvisorObj in allTripAdvisorObjs)
            {
                log.Debug($"Detail extraction progress: {i} out of {allTripAdvisorObjs.Count}");
                i++;
                try
                {
                    var activity = GetDetailsFromTripAdvisor(tripAdvisorObj);
                    activities.Add(activity);

                }
                catch (Exception e)
                {
                    log.Error("Failed to get details from url " + tripAdvisorObj.Url + "\n Message: " + e.Message + "\n StackTrace: " + e.StackTrace);
                }

            }
            log.Debug($"GetAllActivities: Ended");

        }

        static Activity GetDetailsFromTripAdvisor(dynamic obj)
        {
            log.Debug("Getting details from url: " + obj.url);
            var newActivity = new Activity { Id = Guid.NewGuid() };
            var web = new HtmlWeb();
            HtmlDocument document = web.Load(obj.url);

            var titlePattern = "\"name\" : \"(.+)\"";
            var imagePattern = "\"image\" : \"(.+)\"";
            var urlPattern = "\"url\" : \"(.+)\"";
            var typePattern = "\"@type\" : \"(.+)\"";
            var addressPattern = "\"streetAddress\" : \"(.+)\"";
            var cityPattern = "\"addressLocality\" : \"(.+)\"";
            var postalCodePattern = "\"postalCode\" : \"(.+)\"";
            var rgx = new Regex(titlePattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.Title = rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
            }
            rgx = new Regex(imagePattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.Image = rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
                if(newActivity.Image == "https://static.tacdn.com/img2/branding/rebrand/TA_brand_logo.png")
                {
                    newActivity.Image = null;
                }
            }
            rgx = new Regex(urlPattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.Website = "https://www.tripadvisor.dk/" + rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
                newActivity.Url = "https://www.tripadvisor.dk/" + rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
            }
            rgx = new Regex(typePattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.Category = rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
            }
            rgx = new Regex(addressPattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.Address = rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
            }
            rgx = new Regex(cityPattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.City = rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
            }
            rgx = new Regex(postalCodePattern);
            if (rgx.IsMatch(document.DocumentNode.OuterHtml))
            {
                newActivity.PostalCode = rgx.Match(document.DocumentNode.OuterHtml).Groups[1].Value;
            }
            // Get openinghours
            var allEmenents = document.DocumentNode.Descendants();
            if (allEmenents != null)
            {
                var timeRanges = (from element in allEmenents.AsEnumerable()
                                    where element.Attributes["class"] != null &&
                                    element.Attributes["class"].Value == "timeRange"
                                    select element.InnerText).ToList();
                var dayRanges = (from element in allEmenents.AsEnumerable()
                                 where element.Attributes["class"] != null &&
                                 element.Attributes["class"].Value.Contains("dayRange")
                                 select element.InnerText).ToList();
                if(timeRanges.Count != dayRanges.Count)
                {
                    log.Error("Timeranges and dayranges does not match for url: " + obj.url);
                } else
                {
                    var openingHours = "";
                    for (int i = 0; i < timeRanges.Count; i++)
                    {
                        openingHours += dayRanges[i] + ": " + timeRanges[i] + "\n";
                    }
                    if(openingHours != "")
                    {
                        newActivity.OpenHours = openingHours;
                    }
                }
            }
            // Get category
            var category = document.DocumentNode.Descendants()
                .Where(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "detail")
                .Select(x => x.InnerText)
                .ToList();
            if (category != null)
            {
                newActivity.Category = MapTripAdvisorCategory(category.FirstOrDefault());
            }
            // Get lat long from the address
            var add = (newActivity.Address != null ? newActivity.Address : "") +
                (newActivity.PostalCode != null ? ", " + newActivity.PostalCode : "");
            try
            {
                var latlong = Helper.LatLongFromAddress(add);
                latlong.Wait();
                if (latlong.Result != null)
                {
                    newActivity.Latitude = latlong.Result.Lat;
                    newActivity.Longitude = latlong.Result.Long;

                    // Get website from google maps API
                    var googleDetails = GoogleMaps.GoogleMapsAPI.GetDetailsFromLatLongAndKeyWord(
                        newActivity.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)
                        + "," + newActivity.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        newActivity.Title);
                    googleDetails.Wait();
                    if (googleDetails.Result != null)
                    {
                        if (googleDetails.Result.result.website != null)
                        {
                            newActivity.Website = googleDetails.Result.result.website;
                        }
                        // if no image then try to get from google instead
                        if (newActivity.Image == null &&
                            googleDetails.Result.result.photos != null &&
                           googleDetails.Result.result.photos.FirstOrDefault() != null)
                        {
                            newActivity.Image = GoogleMaps.GoogleMapsAPI.GetImageUrlFromGooglePhotoReference(
                                googleDetails.Result.result.photos.FirstOrDefault().photo_reference);
                        }
                    }
                }
            } catch (Exception e)
            {
                log.Error("Failed to get details from google. " + Environment.NewLine + "Message: " + e.Message +
                    Environment.NewLine + "StackTrace: " + e.StackTrace);
            }
            
            return newActivity;
        }

        static List<dynamic> GetActivitiesFromPage(int offset, string cityUrl)
        {
            log.Debug("Getting activities from tripadvisor with offset: " + offset);
            var res = new List<dynamic>();

            var url = "https://www.tripadvisor.dk/" + cityUrl.Replace("[PAGE]", "");

            if (offset > 0)
            {
                url = $"https://www.tripadvisor.dk/{cityUrl.Replace("[PAGE]","oa" + offset.ToString() + "-")  }#ATTRACTION_LIST";
            }
            log.Debug(url);
            var web = new HtmlWeb();
            var document = web.Load(url);

            var links = document.DocumentNode.SelectNodes("//*[@id=\"ATTR_ENTRY_\"]/div//div/div/div//div[2]/a").ToArray();
            var listings = document.DocumentNode.SelectNodes("//*[@id=\"ATTR_ENTRY_\"]/div[2]/div/div").ToArray();
            foreach (var listing in listings)
            {

                var lisitingObj = (from l in listing.Descendants().AsEnumerable()
                                   where l.Attributes["Class"] != null &&
                                   l.Attributes["Class"].Value == "listing_info"
                                   select
                                   new
                                   {
                                       title = (from c in l.Descendants().AsEnumerable()
                                                where
                                                c.Attributes["Class"] != null &&
                                                c.Attributes["Class"].Value == "listing_title "
                                                select c.InnerText.Replace("\r\n", "").Replace("\n", "").Replace("\r", "")
                                                        ).FirstOrDefault(),
                                       url = (from c in l.Descendants().AsEnumerable()
                                              where
                                              c.Attributes["Class"] != null &&
                                              c.Attributes["Class"].Value == "listing_title "
                                              select c.Descendants().Where(a => a.Attributes["href"] != null)
                                              .Select(a => "https://www.tripadvisor.dk" + a.Attributes["href"].Value)
                                              .FirstOrDefault()
                                                      ).FirstOrDefault()
                                   }).FirstOrDefault();
                res.Add(lisitingObj);
            }
            return res;
        }

        static string MapTripAdvisorCategory(string tripAdvisorCategory)
        {
            if (tripAdvisorCategory.Contains("Kunstgallerier"))
            {
                return "Galleri";
            }
            if (tripAdvisorCategory.Contains("Museer"))
            {
                return "Museum";
            }
            if (tripAdvisorCategory.Contains("Parker") ||
                tripAdvisorCategory.Contains("Natur og parker") ||
                tripAdvisorCategory.Contains("Haver"))
            {
                return "Park eller naturområde";
            }
            if (tripAdvisorCategory.Contains("Teatre") ||
                tripAdvisorCategory.Contains("Koncerter"))
            {
                return "Teater eller koncerthus";
            }
            if (tripAdvisorCategory.Contains("Seværdigheder"))
            {
                return "Seværdighed";
            }
            
            // Default er serværdighed
            return "Seværdighed";
        }

    }
}
