# PROSE HACKATHON

Next Friday we are going to have a guest, Alex Polozov, presenting some of his work on program synthesis at UW and Microsoft.

**Time**: Friday Jan 20, 1 PM.

**Title**: PROSE: Inductive Program Synthesis for the Mass Markets

**Speaker**: Alex Polozov, University of Washington / Microsoft.

## Talk details

Programming by example (PBE), or inductive synthesis is a subfield of program synthesis where specification on a target program is provided in a form of input-output examples or, more generally, constraints on the output.
In the last decade, it gained prominence thanks to the mass-market deployments of several PBE-based technologies for data wrangling — the widespread problem of cleaning raw datasets into a structured form, more amenable to analysis.
These technologies include FlashFill, shipped in Microsoft Excel 2013, FlashExtract, shipped in PowerShell and Azure Log Analytics, and others.

Deployment of a mass-market industrial application of program synthesis is challenging.
First, an efficient implementation requires non-trivial engineering insight, often overlooked in a research prototype.
Second, its development should be fast and agile, tailoring to versatile requirements of the users.
Third, the underlying synthesis algorithm should be accessible to the engineers responsible for product maintenance.
These challenges prompted us to generalize the ideas of FlashFill and its successors into PROSE ("PROgram Synthesis using Examples") – a framework for automatic generation of domain-specific inductive synthesizers.
It is the first program synthesis framework that explicitly separates domain-agnostic search algorithms from domain-specific algorithmic insight, making the resulting synthesizer both fast and accessible.
Moreover, PROSE made development of domain-specific PBE applications scalable, allowing us to create a production-ready synthesis library in a few weeks instead of months.

In 2015, our group moved from Microsoft Research to Microsoft Data Group, establishing the first R&D group in PBE-based data wrangling at the company.
There, our unique position as developers of new synthesis APIs for various product teams allows us to collect new problems, insights, and lessons in program synthesis that arise only in end-user facing and industrially deployed applications.
In this talk, I will (a) present the technical insight behind the synthesis algorithms in the PROSE framework, and (b) discuss some of these novel problems and lessons we have learned as we observe how end users interact with the PBE-based applications in the wild.
