
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace SudokuExamProject.Controllers
{
    public class BlogController : Controller
    {
        private static readonly List<BlogArticle> Articles = new()
        {
            new BlogArticle
            {
                Slug = "sudoku-basics",
                Title = "Sudoku nədir? Yeni başlayanlar üçün bələdçi",
                Category = "Başlanğıc",
                Date = "20.10.2025",
                Introduction = "Sudoku rəqəmlərlə oynanılan məntiq oyunudur. Məqsəd boş xanaları qaydalara uyğun doldurmaqdır.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Sudokunun əsas qaydaları",
                        Paragraphs = new()
                        {
                            "Sudoku adətən 9x9 xanadan ibarət cədvəldə oynanılır. Cədvəl doqquz kiçik 3x3 bloka bölünür.",
                            "Hər sətirdə, sütunda və 3x3 blokda 1-dən 9-a qədər rəqəmlər yalnız bir dəfə yerləşdirilməlidir."
                        },
                        Points = new()
                        {
                            "Eyni sətirdə rəqəmləri təkrarlamayın.",
                            "Eyni sütunda rəqəmləri təkrarlamayın.",
                            "Hər 3x3 blokda rəqəmləri təkrarlamayın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "basic-techniques",
                Title = "Sudokunun əsas həll üsulları",
                Category = "Texnikalar",
                Date = "11.10.2026",
                Introduction = "Sudoku həll etmək üçün ilk növbədə cədvəli diqqətlə yoxlamaq və mümkün rəqəmləri müəyyənləşdirmək lazımdır.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Sətir, sütun və blok yoxlaması",
                        Paragraphs = new()
                        {
                            "Boş xanaya rəqəm yazmazdan əvvəl həmin xananın yerləşdiyi sətri, sütunu və 3x3 bloku yoxlayın.",
                            "Bu üç sahədə artıq olan rəqəmlər həmin xanaya yerləşdirilə bilməz."
                        },
                        Points = new()
                        {
                            "Əvvəlcə çoxlu rəqəmi olan sətirləri yoxlayın.",
                            "Boş xanalar üçün mümkün rəqəmləri müəyyən edin.",
                            "Yalnız məntiqi əsas olduqda rəqəm yerləşdirin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "daily-practice",
                Title = "Gündəlik Sudoku məşqləri",
                Category = "Məşq",
                Date = "01.11.2026",
                Introduction = "Davamlı məşq Sudoku bacarıqlarını inkişaf etdirməyə və müxtəlif həll üsullarını daha tez tanımağa kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Gündəlik məşq planı",
                        Paragraphs = new()
                        {
                            "Hər gün 10-15 dəqiqə Sudoku həll etməklə başlaya bilərsiniz. Vaxt keçdikcə daha mürəkkəb tapmacalara keçin.",
                            "Məşqdən sonra hansı üsullardan istifadə etdiyinizi və hansı səhvləri etdiyinizi nəzərdən keçirin."
                        },
                        Points = new()
                        {
                            "İlk olaraq asan tapmaca seçin.",
                            "Məşq üçün müntəzəm vaxt ayırın.",
                            "İrəliləyişinizi qeyd edin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "solve-faster",
                Title = "Sudokunu daha sürətli həll etməyin yolları",
                Category = "Məsləhətlər",
                Date = "25.09.2020",
                Introduction = "Sürətli həllin əsasını təxmin etmək deyil, düzgün müşahidə və sistemli məntiq təşkil edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Səmərəli həll strategiyası",
                        Paragraphs = new()
                        {
                            "Əvvəlcə daha çox rəqəm yerləşdirilmiş sətir, sütun və blokları yoxlayın. Belə sahələrdə boş xanalar üçün uyğun rəqəmi tapmaq daha asan ola bilər.",
                            "Eyni sahəni səbəbsiz təkrar yoxlamaq əvəzinə, tapdığınız yeni rəqəmlərin digər xanalara təsirini araşdırın."
                        },
                        Points = new()
                        {
                            "Asan yerləşdirmələrdən başlayın.",
                            "Namizəd rəqəmlərdən istifadə edin.",
                            "Təxminlərə deyil, sübutlara əsaslanın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "difficulty-levels",
                Title = "Sudokunun çətinlik səviyyələri",
                Category = "Başlanğıc",
                Date = "07.05.2023",
                Introduction = "Sudoku tapmacaları müxtəlif çətinlik səviyyələrində hazırlanır. Uyğun səviyyə seçmək öyrənmə prosesini asanlaşdırır.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Hansı səviyyəni seçmək lazımdır?",
                        Paragraphs = new()
                        {
                            "Asan tapmacalarda sadə yerləşdirmələr daha çox olur. Çətin tapmacalarda isə namizəd rəqəmlər və qabaqcıl məntiq üsulları lazım gələ bilər."
                        },
                        Points = new()
                        {
                            "Asan: yeni başlayanlar üçün.",
                            "Orta: əsas üsulları bilənlər üçün.",
                            "Çətin: daha çox məntiqi təhlil tələb edən tapmacalar üçün.",
                            "Ekspert: mürəkkəb həll üsullarını tətbiq edənlər üçün."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "advanced-techniques",
                Title = "Sudoku üçün qabaqcıl texnikalar",
                Category = "Qabaqcıl texnikalar",
                Date = "18.01.2021",
                Introduction = "Qabaqcıl üsullar sadə yerləşdirmələrin kifayət etmədiyi tapmacalarda namizədlər arasındakı əlaqələri təhlil etməyə kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Məntiqi əlaqələr",
                        Paragraphs = new()
                        {
                            "Cüt namizədlər, X-Wing və digər üsullar müəyyən rəqəmlərin harada yerləşə biləcəyini müqayisə etməyə əsaslanır.",
                            "Bu üsulları tətbiq etməzdən əvvəl mümkün rəqəmləri düzgün qeyd etmək vacibdir."
                        },
                        Points = new()
                        {
                            "Namizədləri diqqətlə qeyd edin.",
                            "Eyni rəqəmin mümkün mövqelərini müqayisə edin.",
                            "Hər nəticəni Sudoku qaydaları ilə yoxlayın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-history",
                Title = "Sudokunun tarixi",
                Category = "Tarix",
                Date = "19.07.2025",
                Introduction = "Sudokunun müasir forması XX əsrin sonlarında geniş yayılmağa başlamış və Yaponiyada böyük populyarlıq qazanmışdır.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Sudoku necə məşhurlaşdı?",
                        Paragraphs = new()
                        {
                            "Sudokunun müasir quruluşuna bənzəyən rəqəm tapmacası 1979-cu ildə ABŞ-da nəşr edilmişdir. Daha sonra oyun Yaponiyada yayılmış və Sudoku adı ilə tanınmışdır.",
                            "2000-ci illərdə qəzetlərdə və rəqəmsal platformalarda yayılması onun beynəlxalq populyarlığını artırmışdır."
                        },
                        Points = new()
                        {
                            "Sudoku rəqəmlərlə oynanılan məntiq tapmacasıdır.",
                            "Oyunda riyazi hesablamadan daha çox məntiqi təhlil vacibdir."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-benefits",
                Title = "Sudoku oynamağın faydaları",
                Category = "Faydalar",
                Date = "31.08.2019",
                Introduction = "Sudoku diqqət, səbir və məntiqi düşünmə bacarıqlarını məşq etdirmək üçün maraqlı fəaliyyətdir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Sudoku hansı bacarıqları məşq etdirir?",
                        Paragraphs = new()
                        {
                            "Tapmacanı həll edərkən mümkün variantları müqayisə edir, qaydaları tətbiq edir və addımlarınızı planlaşdırırsınız.",
                            "Sudoku əyləncəli zehni məşq ola bilər, lakin onun bütün idrak bacarıqlarını mütləq artırdığına dair qəti nəticə çıxarmaq olmaz."
                        },
                        Points = new()
                        {
                            "Diqqəti toplamaq.",
                            "Məntiqi düşünmək.",
                            "Səbirli olmaq.",
                            "Problemləri mərhələlərlə həll etmək."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-mistakes",
                Title = "Sudoku həll edərkən edilən səhvlər",
                Category = "Məsləhətlər",
                Date = "12.11.2026",
                Introduction = "Kiçik diqqətsizliklər Sudoku həllini çətinləşdirə bilər. Ən çox rast gəlinən səhvləri tanımaq daha sistemli işləməyə kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Səhvlərdən necə qaçmaq olar?",
                        Paragraphs = new()
                        {
                            "Ən çox rast gəlinən səhvlərdən biri rəqəmi sətir, sütun və blok qaydalarını tam yoxlamadan yerləşdirməkdir.",
                            "Namizəd rəqəmləri köhnə məlumat əsasında saxlamaq da yanlış nəticələrə səbəb ola bilər."
                        },
                        Points = new()
                        {
                            "Hər rəqəmi yerləşdirməzdən əvvəl yoxlayın.",
                            "Yeni rəqəm yerləşdirildikdə namizədləri yeniləyin.",
                            "Əmin olmadığınız halda rəqəmi təsadüfi seçməyin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-for-students",
                Title = "Şagirdlər üçün Sudoku",
                Category = "Təhsil",
                Date = "21.02.2023",
                Introduction = "Sudoku şagirdlərə məntiqi düşünməyi və qaydalar əsasında problem həll etməyi məşq etdirmək üçün istifadə oluna bilər.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Öyrənməyə necə başlamaq olar?",
                        Paragraphs = new()
                        {
                            "Şagirdlər əvvəlcə sadə tapmacalarla başlamalı, qaydaları başa düşdükdən sonra daha mürəkkəb tapmacalara keçməlidirlər."
                        },
                        Points = new()
                        {
                            "Sadə tapmacalar seçin.",
                            "Hər addımın səbəbini izah edin.",
                            "Səhvləri öyrənmə fürsəti kimi qəbul edin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-mobile",
                Title = "Telefonda Sudoku oynamaq",
                Category = "Rəqəmsal Sudoku",
                Date = "21.10.2026",
                Introduction = "Mobil Sudoku tətbiqləri tapmacaları istənilən yerdə həll etməyə və məşq nəticələrini izləməyə imkan verir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Tətbiq seçərkən nələrə diqqət etməli?",
                        Paragraphs = new()
                        {
                            "Rahat idarəetmə, aydın rəqəmlər, müxtəlif çətinlik səviyyələri və istəyə bağlı səhv yoxlaması faydalı xüsusiyyətlərdir."
                        },
                        Points = new()
                        {
                            "Oxunaqlı interfeys seçin.",
                            "Çətinlik səviyyəsini özünüzə uyğunlaşdırın.",
                            "Oyun zamanı fasilə verməyi unutmayın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-candidates",
                Title = "Namizəd rəqəmlər nədir?",
                Category = "Texnikalar",
                Date = "21.10.2026",
                Introduction = "Namizəd rəqəmlər boş xanaya qaydalara uyğun yerləşdirilə bilən mümkün rəqəmlərdir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Namizədlər necə müəyyən edilir?",
                        Paragraphs = new()
                        {
                            "Boş xananın yerləşdiyi sətirdə, sütunda və 3x3 blokda olan rəqəmləri yoxlayın. İstifadə edilmiş rəqəmlər namizəd ola bilməz.",
                            "Məsələn, həmin sahələrdə 1, 2, 4, 5, 7, 8 və 9 varsa, mümkün namizədlər 3 və 6 ola bilər."
                        },
                        Points = new()
                        {
                            "Mümkün rəqəmləri qeyd edin.",
                            "Yeni rəqəm yerləşdirildikdə namizədləri yeniləyin.",
                            "Yalnız bir namizəd qaldıqda onu yerləşdirmək mümkündür."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "single-candidate",
                Title = "Yeganə namizəd üsulu",
                Category = "Texnikalar",
                Date = "21.10.2026",
                Introduction = "Yeganə namizəd üsulu boş xanada yalnız bir mümkün rəqəm qaldıqda tətbiq edilir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Bu üsul necə işləyir?",
                        Paragraphs = new()
                        {
                            "Xananın sətrini, sütununu və blokunu yoxladıqdan sonra yalnız bir rəqəm mümkün qalırsa, həmin rəqəm xanaya yerləşdirilə bilər."
                        },
                        Points = new()
                        {
                            "Boş xananı seçin.",
                            "Mümkün rəqəmləri yoxlayın.",
                            "Yeganə uyğun rəqəmi yerləşdirin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "hidden-single",
                Title = "Gizli tək rəqəm üsulu",
                Category = "Texnikalar",
                Date = "21.10.2026",
                Introduction = "Gizli tək rəqəm müəyyən sətir, sütun və ya blokda yalnız bir xanaya uyğun gələn rəqəmdir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Gizli tək rəqəmi tapmaq",
                        Paragraphs = new()
                        {
                            "Xana bir neçə namizədə malik ola bilər. Lakin müəyyən blokda konkret rəqəm yalnız həmin xanada yerləşə bilirsə, bu rəqəm gizli tək rəqəm sayılır."
                        },
                        Points = new()
                        {
                            "Bir sətir və ya blok seçin.",
                            "Hər rəqəmin mümkün mövqelərini yoxlayın.",
                            "Yalnız bir mövqeyi olan rəqəmi yerləşdirin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-pairs",
                Title = "Cüt namizədlər üsulu",
                Category = "Texnikalar",
                Date = "21.10.2026",
                Introduction = "Cüt namizədlər üsulu iki xananın eyni iki mümkün rəqəmə malik olduğu hallarda istifadə edilir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Cütləri necə müəyyən etmək olar?",
                        Paragraphs = new()
                        {
                            "Eyni sətirdə, sütunda və ya blokda yerləşən iki xana yalnız eyni iki namizədə malikdirsə, həmin iki rəqəm başqa xanalardan çıxarıla bilər."
                        },
                        Points = new()
                        {
                            "İki eyni namizəd dəstini tapın.",
                            "Xanaların eyni sahədə olmasını yoxlayın.",
                            "Həmin rəqəmləri digər uyğun xanalardan çıxarın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-x-wing",
                Title = "X-Wing Sudoku texnikası",
                Category = "Qabaqcıl texnikalar",
                Date = "21.10.2026",
                Introduction = "X-Wing müəyyən rəqəmin iki sətirdə eyni iki sütunda namizəd olduğu vəziyyətlərdə tətbiq olunan qabaqcıl üsuldur.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "X-Wing necə işləyir?",
                        Paragraphs = new()
                        {
                            "Müəyyən rəqəm iki sətirdə yalnız eyni iki sütunda yerləşə bilirsə, həmin sütunlardakı digər sətirlərin xanalarından bu rəqəmi çıxarmaq mümkündür."
                        },
                        Points = new()
                        {
                            "Eyni rəqəmin namizədlərini izləyin.",
                            "İki sətirdə eyni sütun cütünü tapın.",
                            "Nəticəni bütün əlaqəli xanalar üzrə yoxlayın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-y-wing",
                Title = "Y-Wing Sudoku texnikası",
                Category = "Qabaqcıl texnikalar",
                Date = "21.10.2026",
                Introduction = "Y-Wing üç xana arasında namizəd rəqəmlərin əlaqəsinə əsaslanan qabaqcıl Sudoku üsuludur.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Məntiqi əlaqəni tapmaq",
                        Paragraphs = new()
                        {
                            "Y-Wing adətən iki namizədə malik üç xana arasında xüsusi əlaqə olduqda tətbiq edilir. Düzgün quruluş müəyyən rəqəmin başqa xanadan çıxarılmasına imkan verir."
                        },
                        Points = new()
                        {
                            "Namizəd dəstlərini müqayisə edin.",
                            "Xanaların bir-birini görmə əlaqəsini yoxlayın.",
                            "Rəqəmi yalnız məntiqi nəticə əsaslandırdıqda çıxarın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-time-management",
                Title = "Sudoku həllində vaxtın idarə edilməsi",
                Category = "Məsləhətlər",
                Date = "21.10.2026",
                Introduction = "Vaxtı düzgün bölmək tapmacanın həlli zamanı diqqəti qorumağa və təkrar yoxlamaları azaltmağa kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Vaxtdan səmərəli istifadə",
                        Paragraphs = new()
                        {
                            "Əvvəlcə asan yerləşdirmələri edin. Çətin hissədə uzun müddət dayanırsınızsa, başqa sətir və ya bloka keçin."
                        },
                        Points = new()
                        {
                            "Məşq üçün uyğun vaxt müəyyən edin.",
                            "Çətin xanaya ilişib qalmayın.",
                            "Dəqiqliyi sürətdən üstün tutun."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-difficulty-choice",
                Title = "Özünüzə uyğun Sudoku səviyyəsini seçin",
                Category = "Məsləhətlər",
                Date = "21.10.2026",
                Introduction = "Düzgün çətinlik səviyyəsi seçmək həm öyrənməyi asanlaşdırır, həm də motivasiyanı qorumağa kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Səviyyəni necə müəyyən etmək olar?",
                        Paragraphs = new()
                        {
                            "Əgər tapmacaları əsasən təxmin etmədən həll edə bilirsinizsə, bir qədər çətin səviyyəyə keçməyi sınaya bilərsiniz."
                        },
                        Points = new()
                        {
                            "Yeni başlayırsınızsa, asan səviyyədən başlayın.",
                            "Əsas üsulları mənimsədikdə orta səviyyəyə keçin.",
                            "Çətin tapmacalarda yeni üsulları öyrənin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-printable",
                Title = "Çap edilə bilən Sudoku tapmacaları",
                Category = "Məşq",
                Date = "21.10.2026",
                Introduction = "Kağız üzərində Sudoku həll etmək rəqəmləri və namizədləri əl ilə qeyd etməyə imkan verir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Kağız üzərində məşq",
                        Paragraphs = new()
                        {
                            "Çap edilmiş tapmacalar ekran qarşısında olmaq istəməyənlər üçün alternativdir. Qələmlə işləyərkən səhvləri düzəltmək üçün ehtiyatlı qeyd aparmaq faydalıdır."
                        },
                        Points = new()
                        {
                            "Aydın çap edilmiş cədvəl seçin.",
                            "Namizədləri kiçik və oxunaqlı yazın.",
                            "Həll tamamlandıqda bütün qaydaları yoxlayın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-online-safety",
                Title = "Onlayn Sudoku oynayarkən təhlükəsizlik",
                Category = "Təhlükəsizlik",
                Date = "21.10.2026",
                Introduction = "Onlayn oyunlardan istifadə edərkən şəxsi məlumatların qorunmasına və etibarlı platforma seçiminə diqqət yetirmək vacibdir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Təhlükəsiz oyun təcrübəsi",
                        Paragraphs = new()
                        {
                            "Sadə Sudoku oynamaq üçün adətən həssas şəxsi məlumatlar lazım olmur. Saytın etibarlılığını yoxlayın və lazımsız icazələr verməyin."
                        },
                        Points = new()
                        {
                            "Şəxsi məlumatlarınızı paylaşmayın.",
                            "Şübhəli keçidlərə klikləməyin.",
                            "Tətbiqləri etibarlı mənbələrdən əldə edin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-daily-routine",
                Title = "Sudoku üçün gündəlik rutin",
                Category = "Məşq",
                Date = "21.10.2026",
                Introduction = "Qısa və davamlı məşq planı Sudoku bacarıqlarını tədricən inkişaf etdirmək üçün əlverişlidir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Sadə gündəlik plan",
                        Paragraphs = new()
                        {
                            "Gündə bir asan tapmaca ilə başlayın. Təcrübə artdıqca məşqə orta və çətin tapmacalar əlavə edin."
                        },
                        Points = new()
                        {
                            "Məşq üçün sabit vaxt seçin.",
                            "Həll etdiyiniz tapmacaları qeyd edin.",
                            "Yeni bir üsulu öyrənməyə vaxt ayırın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-advanced-strategy",
                Title = "Çətin Sudoku üçün strategiyalar",
                Category = "Qabaqcıl texnikalar",
                Date = "21.10.2026",
                Introduction = "Çətin Sudoku tapmacalarında namizədləri sistemli izləmək və bir neçə məntiqi əlaqəni birlikdə təhlil etmək lazım gələ bilər.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Çətin tapmacaya yanaşma",
                        Paragraphs = new()
                        {
                            "Sadə üsulları yenidən yoxladıqdan sonra cütlər, xətlər və qabaqcıl namizəd əlaqələrini araşdırın."
                        },
                        Points = new()
                        {
                            "Namizədləri səliqəli qeyd edin.",
                            "Eyni rəqəmin mövqelərini müqayisə edin.",
                            "Hər nəticənin səbəbini müəyyən edin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-focus",
                Title = "Sudoku zamanı diqqəti necə toplamaq olar?",
                Category = "Məsləhətlər",
                Date = "21.10.2026",
                Introduction = "Diqqətli və sistemli yanaşma rəqəmləri daha düzgün yoxlamağa kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Diqqəti qorumaq üçün məsləhətlər",
                        Paragraphs = new()
                        {
                            "Sakit mühit seçin, tapmacanı kiçik hissələr üzrə həll edin və ehtiyac olduqda qısa fasilələr verin."
                        },
                        Points = new()
                        {
                            "Bir anda bir sətir və ya blok üzərində işləyin.",
                            "Hər addımın məntiqini yoxlayın.",
                            "Tələsmədən irəliləyin."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-puzzles-for-family",
                Title = "Ailə ilə Sudoku oynamaq",
                Category = "Əyləncə",
                Date = "21.10.2026",
                Introduction = "Sudoku ailə üzvlərinin birlikdə məntiqi tapmacalar həll edə və müxtəlif həll yollarını müzakirə edə biləcəyi əyləncəli fəaliyyətdir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "Birlikdə necə oynamaq olar?",
                        Paragraphs = new()
                        {
                            "Ailə üzvləri növbə ilə bir sətir, sütun və ya blok üzrə mümkün rəqəmləri izah edə bilərlər. Əsas məqsəd yarışmaqdan çox, birlikdə düşünməkdir."
                        },
                        Points = new()
                        {
                            "Hamı üçün uyğun səviyyə seçin.",
                            "Həll yollarını bir-birinizə izah edin.",
                            "Düzgün nəticəni birlikdə yoxlayın."
                        }
                    }
                }
            },

            new BlogArticle
            {
                Slug = "sudoku-progress-tracking",
                Title = "Sudoku üzrə irəliləyişi izləmək",
                Category = "Məşq",
                Date = "21.10.2026",
                Introduction = "İrəliləyişi izləmək hansı üsulları öyrəndiyinizi və hansı mövzularda daha çox məşq etməli olduğunuzu müəyyənləşdirməyə kömək edir.",
                Sections = new()
                {
                    new BlogSection
                    {
                        Heading = "İnkişafınızı necə ölçmək olar?",
                        Paragraphs = new()
                        {
                            "Təkcə həll vaxtını deyil, tapmacanı nə qədər düzgün və hansı üsullarla həll etdiyinizi də qeyd edin."
                        },
                        Points = new()
                        {
                            "Həll etdiyiniz tapmacaların sayını yazın.",
                            "Çətinlik səviyyəsini qeyd edin.",
                            "Öyrəndiyiniz yeni üsulları siyahıya alın."
                        }
                    }
                }
            }
        };

        public IActionResult Index()
        {
            return View(Articles);
        }

        public IActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            var article = Articles.FirstOrDefault(
                a => a.Slug == slug);

            if (article == null)
            {
                return NotFound();
            }

            int index = Articles.IndexOf(article);

            ViewBag.PreviousArticle =
                index > 0 ? Articles[index - 1] : null;

            ViewBag.NextArticle =
                index < Articles.Count - 1
                    ? Articles[index + 1]
                    : null;

            ViewBag.RelatedArticles = Articles
                .Where(a => a.Slug != article.Slug)
                .Where(a => a.Category == article.Category)
                .Take(3)
                .ToList();

            return View(article);
        }
    }
}
