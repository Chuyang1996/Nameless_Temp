Shader "Sbin/ff2" {
    // 贴图采样
    properties{
        // 变量名("描述名",类型)=值
        _Color("主体", color) = (1,1,1,1)
        _Ambient("环境光", color) = (0.3,0.3,0.3,0.3)
        _Specular("高光", color) = (1,1,1,1)

        // 变量名("描述名",range(区域最小值,区域最大值)=默认值
        _Shininess("高光强度",range(0,8)) = 4
        _Emission("自发光", color) = (1,1,1,1)

        _Constant("透明通道", color) = (1,1,1,0.3)

        _MainTex("纹理", 2d) = ""
        _SecondTex("第二张纹理",2d) = ""


    }

        SubShader{
    Tags { "Queue" = "Transparent" }


            pass {

                Blend SrcAlpha OneMinusSrcAlpha

                material {
                    diffuse[_Color]
                    ambient[_Ambient]
                    specular[_Specular]
                    shininess[_Shininess]
                    emission[_Emission]
                }
                lighting on // 启用光照
                separatespecular on  // 镜面高光

                // 纹理属性
                settexture[_MainTex] {
                    // 合并 当前纹理 * 前面所有材质和关照的颜色
                    // primary 代表顶点光照后的颜色
                    // double 颜色*2
                    // quad 颜色*4
                    combine texture * primary double
                }

                    // 第二张纹理
                    settexture[_SecondTex] {
                    // 用当前采用到的纹理与之前所有采样到的结果进行混合        

                    //combine texture * previous double

                    // , 号后面的参数，它只是取了纹理alpha通道, 前面所有的颜色alpha值失效
                    constantcolor[_Constant]
                    combine texture * previous double, texture * constant
                }
            }
    }
        // FallBack "Diffuse"
}