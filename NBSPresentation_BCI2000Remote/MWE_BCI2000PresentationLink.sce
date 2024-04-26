#-- scenario file --#

scenario = "MWE_BCI2000PresentationLink";
pcl_file = "MWE_BCI2000PresentationLink.pcl";

begin;

picture {} default;
box { height = 400; width = 400; color = 255,255,255; } Square;

trial {
	trial_duration = 1000;
	
	picture {
      box Square;
      x = 0; y = 0;
   };
	
} main;
